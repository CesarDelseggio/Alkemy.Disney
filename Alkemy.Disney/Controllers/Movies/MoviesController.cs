using Alkemy.Disney.Models;
using Alkemy.Disney.Models.Entities;
using Alkemy.Disney.DTOs.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alkemy.Disney.DTOs.Caracters;

namespace Alkemy.Disney.Controllers.Movies
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private DisneyDbContext _db;

        public MoviesController(DisneyDbContext context)
        {
            _db = context;
        }

        // GET: Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoviesDTO>>> Get(string title = null, int? genre = null, string order = "ASC")
        {
            var query = _db.Movies.AsQueryable();

            if(title != null) { query = query.Where(x => x.Title.Contains(title)); }

            if(genre != null) { query = query.Where(x => x.Genre.Id == genre); }

            //Por defecto ordena de forma ascendente.
            if(order?.ToUpper() == "DESC") { 
                query = query.OrderByDescending(x=>x.CreationDate); 
            }else{
                query = query.OrderBy(x => x.CreationDate);
            }

            return await query.Include(x=>x.Caracters)
                            .Select(x=> new MoviesDTO(x))
                            .ToListAsync();
        }

        // GET: Movies/1
        [HttpGet("{id}")]
        public async  Task<ActionResult<MovieDetailDTO>> Get(int id)
        {
            var entity = await _db.Movies.Include(x=>x.Caracters)
                .Include(x=>x.Genre)
                .FirstOrDefaultAsync(x=>x.Id==id);

            if (entity == null) { return NotFound(); }

            return new MovieDetailDTO(entity);
        }

        // POST: Movies
        [HttpPost]
        public async Task<ActionResult<MovieDetailDTO>> Post([FromForm] MovieEditDTO entityDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (entityDTO?.Id != 0)
            {
                ModelState.AddModelError(nameof(Movie.Id), "El id es autogenerado.");
                return BadRequest(ModelState);
            }

            if (!ExistGenre(entityDTO.GenreId)) {
                ModelState.AddModelError(nameof(Movie.GenreId), "No se encontro el género especifocado"); 
                return BadRequest(ModelState); 
            }

            var entity = new Movie();
            entityDTO.ToModel(entity);
            _db.Movies.Add(entity);
            await _db.SaveChangesAsync();

            var result = new MovieDetailDTO(entity);
            return CreatedAtAction(nameof(MoviesController.Get), new {id = entityDTO.Id}, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieEditDTO entityDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (entityDTO.Id != id)
            {
                ModelState.AddModelError(nameof(id), $"El id no se puede modificar, debe ser igual a {id}");
                return BadRequest(ModelState);
            }

            var entity = _db.Movies.Find(entityDTO.Id);
            if (entity == null) { return NotFound(); }

            try
            {
                entityDTO.ToModel(entity);
                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exist(id)) { return NotFound(); } else { throw; }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _db.Movies.FindAsync(id);

            if(entity is null) { return NotFound(); }  

            _db.Movies.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet]
        [Route("{id}/[action]")]
        public async Task<ActionResult<IEnumerable<CaractersDTO>>> Caracters(int id)
        {
            Movie entity = await _db.Movies.Include(x => x.Caracters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null) { return NotFound(); }

            return entity.Caracters
                    .Select(x => new CaractersDTO(x)).ToList();
        }

        [HttpPost]
        [Route("{id}/[action]")]
        public async Task<ActionResult> Caracters(int id, [FromForm] BaseEntity caracter)
        {
            Movie entity = await _db.Movies.Include(x => x.Caracters)
                .FirstOrDefaultAsync(x => x.Id == id);
            Caracter caracterToAdd = await _db.Caracters.FindAsync(caracter.Id);

            if (entity is null || caracterToAdd is null)
            {
                ModelState.AddModelError(nameof(id), $"Debe especificar un personaje y pelícuala válido");
                return NotFound();
            }

            try
            {
                if (!entity.Caracters.Contains(caracterToAdd))
                {
                    entity.Caracters.Add(caracterToAdd);
                    await _db.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exist(id)) { return NotFound(); } else { throw; }
            }

            return NoContent();
        }


        [HttpDelete]
        [Route("{id}/[action]/{idCaracter}")]
        public async Task<ActionResult> Caracters(int id, int idCaracter)
        {
            Movie movie = await _db.Movies.Include(x => x.Caracters)
                .FirstOrDefaultAsync(x => x.Id == id);
            Caracter caracter = await _db.Caracters.FindAsync(idCaracter);

            if (movie is null || caracter is null)
            {
                ModelState.AddModelError(nameof(id), $"Debe especificar un personaje y pelícuala válido");
                return NotFound();
            }

            try
            {
                if (movie.Caracters.Contains(caracter))
                {
                    movie.Caracters.Remove(caracter);
                    await _db.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exist(id)) { return NotFound(); } else { throw; }
            }

            return NoContent();
        }

        private bool Exist(int id)
        {
            return _db.Movies.Any(x=>x.Id == id);
        }

        private bool ExistGenre(int id)
        {
            return _db.Genres.Any(x => x.Id == id);
        }
    }
}
