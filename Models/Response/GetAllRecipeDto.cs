using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class GetAllRecipeDto
    {
        public int total { get; set; }
        public List<AllRecipe> recipes { get; set; }
    }

    public class AllRecipe
    {
        public int id { get; set; }
        public string name { get; set; }
        public int recipeCategoryId { get; set; }
        public string image { get; set; }
        public int? nReactionLike { get; set; } = 0;
        public int? nReactionNeutral { get; set; } = 0;
        public int? nReactionDislike { get; set; } = 0;
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Category recipeCategory { get; set; }
    }
}
