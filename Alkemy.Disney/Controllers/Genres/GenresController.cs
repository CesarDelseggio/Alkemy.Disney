using Alkemy.Disney.Models;
using Alkemy.Disney.Models.Entities;
using Alkemy.Disney.DTOs.Genres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Alkemy.Disney.Controllers.Genres
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private DisneyDbContext _db;
        
        public GenresController(DisneyDbContext context)
        {
            _db = context;
        }

        // GET: Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenresDTO>>> Get()
        {
            return await _db.Genres
                         .Select(x => new GenresDTO(x))
                         .ToListAsync();
        }

        // GET: Genres/1
        [HttpGet("{id}")]
        public async  Task<ActionResult<GenreDetailDTO>> Get(int id)
        {
            var entity = await _db.Genres
                                .Include(x => x.Movies)
                                .FirstOrDefaultAsync(x=>x.Id == id);

            if (entity == null) { return NotFound(); }

            return new GenreDetailDTO(entity);
        }

        // POST: Genres
        [HttpPost]
        public async Task<ActionResult<GenreDetailDTO>> Post([FromForm] GenreEditDTO entityDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (entityDTO?.Id != 0)
            {
                ModelState.AddModelError(nameof(Caracter.Id), "El id es autogenerado.");
                return BadRequest(ModelState);
            }

            var entity = new Genre();
            entityDTO.ToModel(entity);
            _db.Genres.Add(entity);
            await _db.SaveChangesAsync();

            var result = new GenreDetailDTO(entity);
            return CreatedAtAction(nameof(GenresController.Get), new {id = result.Id}, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] GenreEditDTO entityDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (entityDTO.Id != id)
            {
                ModelState.AddModelError(nameof(id), $"El id no se puede modificar, debe ser igual a {id}");
                return BadRequest(ModelState);
            }

            var entity = _db.Genres.Find(entityDTO.Id);
            if(entity == null) { return NotFound(); }

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
            var entity = await _db.Genres.FindAsync(id);

            if(entity is null) { return NotFound(); }  

            _db.Genres.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool Exist(int id)
        {
            return _db.Genres.Any(x=>x.Id == id);
        }
    }
}
