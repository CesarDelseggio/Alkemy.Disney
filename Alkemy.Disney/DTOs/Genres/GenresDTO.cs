using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;

namespace Alkemy.Disney.DTOs.Genres
{
    public class GenresDTO
    {
        public GenresDTO()
        {

        }

        public GenresDTO(Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name;
            Image = $"{ImageHelper.GenresURL}/{Id}";
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}
