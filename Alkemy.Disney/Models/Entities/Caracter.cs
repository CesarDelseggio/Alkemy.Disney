using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Alkemy.Disney.Models.Entities
{
    public class Caracter : BaseEntity
    {
        public Caracter()
        {
            this.Movies = new HashSet<Movie>();
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        
        [StringLength(600)]
        public string History { get; set; }
        public byte[] Image { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
