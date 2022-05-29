using Alkemy.Disney.Helpers;
using Alkemy.Disney.Models.Entities;

namespace Alkemy.Disney.DTOs.Caracters
{
    public class CaractersDTO
    {
        public CaractersDTO()
        {

        }

        public CaractersDTO(Caracter caracter)
        {
            Id = caracter.Id;
            Name = caracter.Name;
            Image = $"{ImageHelper.CaractersURL}/{Id}";
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}
