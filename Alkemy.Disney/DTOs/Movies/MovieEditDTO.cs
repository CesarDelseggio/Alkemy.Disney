using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Alkemy.Disney.DTOs.Movies
{
    public class MovieEditDTO
    {
        public MovieEditDTO()
        {

        }

        public MovieEditDTO(Movie movie)
        {
            Id = movie.Id;
            Title = movie.Title;
            CreationDate = movie.CreationDate;
            Rate = movie.Rate;
            GenreId = movie.GenreId;
            Genre = movie.Genre;
            Caracters = movie.Caracters;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        [Range(1, 5)]
        public int Rate { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<Caracter> Caracters { get; set; }

        internal Movie ToModel(Movie entity)
        {
            entity.Id = Id;
            entity.Title = Title;
            entity.CreationDate = CreationDate;
            entity.Rate = Rate;
            entity.GenreId = GenreId;
            //No establecemos las propiedades relacinadas para evitar volver a crearlas.
            //entity.Genre = Genre;
            //entity.Caracters = Caracters;

            var img = GetImageByte();
            if (img != null) { entity.Image = img; }


            return entity;
        }

        internal byte[] GetImageByte()
        {
            if (Image?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    //string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                    return fileBytes;
                }
            };

            return null;
        }
    }
}
