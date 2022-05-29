using Alkemy.Disney.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Alkemy.Disney.DTOs.Caracters
{
    public class CaracterEditDTO
    {
        public CaracterEditDTO()
        {

        }

        public CaracterEditDTO(Caracter genre)
        {
            Id = genre.Id;
            Name = genre.Name;
            Age = genre.Age;
            Weight = genre.Weight;
            History = genre.History;
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

        public IFormFile Image { get; set; }

        internal Caracter ToModel(Caracter entity)
        {
            entity.Id = Id;
            entity.Name = Name;
            entity.Age = Age;
            entity.Weight = Weight;
            entity.History = History;

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
