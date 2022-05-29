using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alkemy.Disney.Models.Entities
{
    public class Movie : BaseEntity
    {
        public Movie()
        {
            this.Caracters = new HashSet<Caracter>();
        }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public byte[] Image { get; set; }

        public ICollection<Caracter> Caracters { get; set; }
    }
}
