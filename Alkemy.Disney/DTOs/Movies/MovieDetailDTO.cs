using Alkemy.Disney.DTOs.Caracters;
using Alkemy.Disney.DTOs.Genres;
using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alkemy.Disney.DTOs.Movies
{
    public class MovieDetailDTO
    {
        public MovieDetailDTO()
        {

        }

        public MovieDetailDTO(Movie entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            CreationDate = entity.CreationDate;
            Rate = entity.Rate;
            GenreId = entity.GenreId;
            
            Image = $"{ImageHelper.MoviesURL}/{Id}";

            if (entity.Genre != null)
            {
                //Evita un bucle infinito por referencia circular.
                entity.Genre.Movies = null;
                Genre = new GenreDetailDTO(entity.Genre);
            }

            if (entity.Caracters != null)
            {
                //Evita un bucle infinito por referencia circular.
                foreach (var item in entity.Caracters)
                {
                    item.Movies = null;
                }
                Caracters = entity.Caracters.Select(x => new CaracterDetailDTO(x)).ToList();
            }
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int Rate { get; set; }
        public int GenreId { get; set; }
        public GenreDetailDTO Genre { get; set; }
        public string Image { get; set; }
        public ICollection<CaracterDetailDTO> Caracters { get; set; }
    }
}
