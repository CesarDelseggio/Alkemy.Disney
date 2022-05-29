using Alkemy.Disney.Models;
using Alkemy.Disney.Models.Entities;
using Alkemy.Disney.DTOs.Caracters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alkemy.Disney.DTOs.Movies;

namespace Alkemy.Disney.Controllers.Caracters
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CaractersController : ControllerBase
    {
        private readonly DisneyDbContext _db;

        public CaractersController(DisneyDbContext context)
        {
            _db = context;
        }

        // GET: Caracters
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CaractersDTO>>> Get(string name = null, int? age = null,
            decimal? weight = null, int? movies = null)
        {
            var query = _db.Caracters.AsQueryable();

            if (name != null) { query = query.Where(x => x.Name.Contains(name)); }

            if (age != null) { query = query.Where(x => x.Age == age); }

            if (weight != null) { query = query.Where(x => x.Weight == weight); }

            if (movies != null) { query = query.Where(x => x.Movies.Any(x => x.Id == movies)); }

            return await query
                .Select(x => new CaractersDTO(x))
                .ToListAsync();
        }

        // GET: Caracters/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CaracterDetailDTO>> Get(int id)
        {
            var entity = await _db.Caracters.Include(x => x.Movies)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) { return NotFound(); }

            return new CaracterDetailDTO(entity);
        }

        // POST: Caracters
        [HttpPost]
        public async Task<ActionResult<CaracterDetailDTO>> Post([FromForm] CaracterEditDTO entityDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (entityDTO?.Id != 0) {
                ModelState.AddModelError(nameof(Caracter.Id), "El id es autogenerado.");
                return BadRequest(ModelState);
            }

            var entity = new Caracter();
            entityDTO.ToModel(entity);
            _db.Caracters.Add(entity);
            await _db.SaveChangesAsync();

            var resul = new CaracterDetailDTO(entity);
            return CreatedAtAction(nameof(CaractersController.Get), new { id = entityDTO.Id }, resul);
        }

        // PUT: Caracters/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] CaracterEditDTO entityDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (entityDTO.Id != id) {
                ModelState.AddModelError(nameof(id), $"El id no se puede modificar, debe ser igual a {id}");
                return BadRequest(ModelState);
            }

            var entity = _db.Caracters.Find(entityDTO.Id);
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

        // DELETE: Caracters/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _db.Caracters.FindAsync(id);

            if (entity is null) { return NotFound(); }

            _db.Caracters.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("{id}/[action]")]
        public async Task<ActionResult<IEnumerable<MoviesDTO>>> Movies(int id)
        {
            Caracter entity = await _db.Caracters.Include(x => x.Movies)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (entity is null) { return NotFound(); }

            return entity.Movies
                    .Select(x => new MoviesDTO(x)).ToList();
        }

        [HttpPost]
        [Route("{id}/[action]")]
        public async Task<ActionResult> Movies(int id, [FromForm] BaseEntity movie)
        {
            if (movie is null) { return BadRequest(); }

            Caracter caracter = await _db.Caracters.Include(x=>x.Movies)
                .FirstOrDefaultAsync(x=>x.Id == id);
            Movie movieToAdd = await _db.Movies.FindAsync(movie.Id);

            if (caracter is null || movieToAdd is null) {
                ModelState.AddModelError(nameof(id), $"Debe especificar un personaje y pelícuala válido");
                return NotFound(); 
            }

            try
            {
                if (!caracter.Movies.Contains(movieToAdd))
                {
                    caracter.Movies.Add(movieToAdd);
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
        [Route("{id}/[action]/{idMovie}")]
        public async Task<ActionResult> Movies(int id, int idMovie)
        {
            Caracter caracter = await _db.Caracters.Include(x => x.Movies)
                .FirstOrDefaultAsync(x => x.Id == id);
            Movie movie = await _db.Movies.FindAsync(idMovie);

            if (caracter is null || movie is null)
            {
                ModelState.AddModelError(nameof(id), $"Debe especificar un personaje y pelícuala válido");
                return NotFound();
            }

            try
            {
                if (caracter.Movies.Contains(movie))
                {
                    caracter.Movies.Remove(movie);
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
            return _db.Caracters.Any(x => x.Id == id);
        }
    }
}
