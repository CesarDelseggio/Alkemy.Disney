using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alkemy.Disney.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
