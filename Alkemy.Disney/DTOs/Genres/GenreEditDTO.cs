using Alkemy.Disney.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Alkemy.Disney.DTOs.Genres
{
    public class GenreEditDTO
    {
        public GenreEditDTO()
        {

        }
        public GenreEditDTO(Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name; 
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IFormFile Image { get; set; }

        internal Genre ToModel(Genre entity)
        {
            entity.Id = Id;
            entity.Name = Name;
            
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
