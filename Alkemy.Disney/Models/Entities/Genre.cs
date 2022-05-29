using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alkemy.Disney.Models.Entities
{
    public class Genre : BaseEntity
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
