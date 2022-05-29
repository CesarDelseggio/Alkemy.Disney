using Alkemy.Disney.DTOs.Movies;
using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alkemy.Disney.DTOs.Caracters
{
    public class CaracterDetailDTO
    {
        public CaracterDetailDTO()
        {

        }


        public CaracterDetailDTO(Caracter entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Age = entity.Age;
            Weight = entity.Weight;
            History = entity.History;
            Image = $"{ImageHelper.CaractersURL}/{Id}";

            if(entity.Movies != null)
            {
                //Evita un bucle infinito por referencia circular.
                foreach (var item in entity.Movies)
                {
                    item.Caracters = null;
                }
                Movies = entity.Movies.Select( x=> new MovieDetailDTO(x)).ToList();
            }
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }

        [StringLength(600)]
        public string History { get; set; }
        public string Image { get; set; }

        public ICollection<MovieDetailDTO> Movies { get; set; }
    }
}
