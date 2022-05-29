using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;
using System;

namespace Alkemy.Disney.DTOs.Movies
{
    public class MoviesDTO
    {
        public MoviesDTO()
        {

        }

        public MoviesDTO(Movie genre)
        {
            Id = genre.Id;
            Title = genre.Title;
            CreationDate = genre.CreationDate;
            Image = $"{ImageHelper.MoviesURL}/{Id}";
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string Image { get; set; }
    }
}
