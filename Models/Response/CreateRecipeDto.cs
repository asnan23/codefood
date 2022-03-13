using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class CreateRecipeDto
    {
        public string name { get; set; }
        public int recipeCategoryId { get; set; }
        public string image { get; set; }
        public int nServing { get; set; } = 1;
        public List<IngredientDto> ingredientsPerServing { get; set; }
        public List<StepDto> steps { get; set; }
        public int id { get; set; }
        public int? nReactionLike { get; set; } = 0;
        public int? nReactionNeutral { get; set; } = 0;
        public int? nReactionDislike { get; set; } = 0;
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
