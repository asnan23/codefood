using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class CreateUpdateRecipe
    {
        [Required]
        public string name { get; set; }
        [Required(ErrorMessage = "Category Id is required")]
        public int recipeCategoryId { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public string image { get; set; }
        [Required(ErrorMessage = "nServing is required")]
        public int nServing { get; set; } = 1;
        public List<IngredientDto> ingredientsPerServing { get; set; }
        public List<StepDto> steps { get; set; }
    }
}
