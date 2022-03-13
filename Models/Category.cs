using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime? updatedAt { get; set; } = DateTime.Now;
    }
}
