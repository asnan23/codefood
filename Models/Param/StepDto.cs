using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class StepDto
    {
        [Required(ErrorMessage = "Step Order is required")]
        public int stepOrder { get; set; }
        public string description { get; set; }
    }

}
