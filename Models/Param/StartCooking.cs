using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Param
{
    public class StartCooking
    {
        [Required(ErrorMessage = "nServing is required")]
       // [RegularExpression("[^1-9]", ErrorMessage = "Invalid target serving")]
        [MinLength(1,ErrorMessage = "Target serving minimum 1")]
        public int nServing { get; set; }
        [Required(ErrorMessage = "recipeId is required")]
       // [RegularExpression("[^1-9]", ErrorMessage = "Invalid recipe id")]
        public int recipeId { get; set; }
    }

    public class UpdateStep
    {
        [Required(ErrorMessage = "stepOrder is required")]
       // [RegularExpression("[^1-9]", ErrorMessage = "Invalid target stepOrder")]
        public int stepOrder { get; set; }
    }

    public class Reaction
    {
        [Required(ErrorMessage = "reaction is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "reaction is invalid")]
        public string reaction { get; set; }
    }
}
