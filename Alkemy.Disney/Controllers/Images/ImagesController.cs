
using Alkemy.Disney.Models;
using Alkemy.Disney.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace Alkemy.Disney.Controllers.Images
{
    [Route("[controller]")]
    [ApiController]
    
    public class ImagesController : ControllerBase
    {
        private DisneyDbContext _db;
        public ImagesController(DisneyDbContext context)
        {
            this._db = context;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> Genres()
        {
            return "Para acceder a las imagenes deba pasar el id como parámetro.";
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult> Genres(int id)
        {
            var entity = await _db.Genres.FindAsync(id);

            if (entity is null || entity.Image is null) { return NotFound(); }

            var file = new FileContentResult(entity.Image, new MediaTypeHeaderValue("image/jpg"));

            return file;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> Caracters()
        {
            return "Para acceder a las imagenes deba pasar el id como parámetro.";
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult> Caracters(int id)
        {
            var entity = await _db.Caracters.FindAsync(id);

            if (entity is null || entity.Image is null) { return NotFound(); }

            var file = new FileContentResult(entity.Image, new MediaTypeHeaderValue("image/jpg"));

            return file;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> Movies()
        {
            return "Para acceder a las imagenes deba pasar el id como parámetro.";
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult> Movies(int id)
        {
            var entity = await _db.Movies.FindAsync(id);

            if (entity is null || entity.Image is null) { return NotFound(); }

            var file = new FileContentResult(entity.Image, new MediaTypeHeaderValue("image/jpg"));

            return file;
        }
    }
}
