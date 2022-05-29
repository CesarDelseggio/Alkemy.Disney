using Alkemy.Disney.DTOs.Movies;
using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Alkemy.Disney.DTOs.Genres
{
    public class GenreDetailDTO
    {
        public GenreDetailDTO()
        {

        }

        public GenreDetailDTO(Genre entity)
        {
            Id=entity.Id;
            Name=entity.Name;
            Image = $"{ImageHelper.GenresURL}/{Id}";

            if (entity.Movies != null)
            {
                //Evita un bucle infinito por referencia circular.
                foreach (var item in entity.Movies)
                {
                    item.Caracters = null;
                    item.Genre = null;
                }
                Movies = entity.Movies.Select(x => new MovieDetailDTO(x)).ToList();
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public ICollection<MovieDetailDTO> Movies { get; set; }
    }
}
