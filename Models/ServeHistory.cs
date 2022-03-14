using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models
{
    public class ServeHistory
    {
        [Key]       
        public int id { get; set; }
        [StringLength(4)]
        public string code { get; set; }
        public int userId { get; set; }
        public int nServing { get; set; }
        public int recipeId { get; set; }
        public int nStep { get; set; }
        public int nStepDone { get; set; }
        public string reaction { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }
}
