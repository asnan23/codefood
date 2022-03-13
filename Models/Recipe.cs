using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models
{
    public class Recipe
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public int recipeCategoryId { get; set; }
        [Required]
        public string image { get; set; }
        public int? nReactionLike { get; set; }
        public int? nReactionNeutral { get; set; }
        public int? nReactionDislike { get; set; }
        [Required]
        public int nServing { get; set; } = 1;
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }
}
