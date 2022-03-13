using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Response;
using CodeFood_API.Asnan.Repository;
using CodeFood_API.Asnan.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Controllers
{
    [Route("/recipes")]
    public class RecipeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        protected SuccessDTO _success;
        protected ErrorDTO _error;
        private readonly IValidationErrorRepository _validationErrorRepository;
        public RecipeController(IUnitOfWork unitOfWork, IValidationErrorRepository validationErrorRepository, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            this._success = new SuccessDTO();
            this._error = new ErrorDTO();
            _validationErrorRepository = validationErrorRepository;
        }
        [HttpPost]
        public IActionResult Post([FromBody] CreateUpdateRecipe model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }
            var category = _unitOfWork.Category.GetFirstOrDefault(t => t.id == model.recipeCategoryId);
            if (category == null)
            {
                _error.message = $"Recipe Category with id {model.recipeCategoryId} not found";
                return BadRequest(_error);
            }
            Recipe obj = new Recipe
            {
                name = model.name,
                recipeCategoryId = model.recipeCategoryId,
                nServing = model.nServing,
                image = model.image
            };
            _unitOfWork.Recipe.Add(obj);
            _unitOfWork.Save();

            foreach (var item in model.ingredientsPerServing)
            {
                Ingredient ingredient = new Ingredient
                {
                    recipeId = obj.id,
                    item = item.item,
                    unit = item.unit,
                    value = item.value
                };
                _unitOfWork.Ingredient.Add(ingredient);
                _unitOfWork.Save();
            }

            foreach (var item in model.steps)
            {
                Step step = new Step
                {
                    recipeId = obj.id,
                    stepOrder = item.stepOrder,
                    description = item.description,
                };
                _unitOfWork.Step.Add(step);
                _unitOfWork.Save();
            }

            var recipe = _db.Recipes.Where(t => t.id == obj.id).FirstOrDefault();

            CreateRecipeDto recipeDto = new CreateRecipeDto
            {
                id = recipe.id,
                name = recipe.name,
                image = recipe.image,
                recipeCategoryId = recipe.recipeCategoryId,
                nServing = recipe.nServing,
                nReactionDislike = recipe.nReactionDislike != null ? recipe.nReactionDislike : 0,
                nReactionLike = recipe.nReactionLike != null ? recipe.nReactionLike : 0,
                nReactionNeutral = recipe.nReactionNeutral != null ? recipe.nReactionNeutral : 0,
                updatedAt = recipe.updatedAt,
                createdAt = recipe.createdAt,
            };

            var ingredients = _db.Ingredients.Where(t => t.recipeId == obj.id).ToList();
            foreach (var item in ingredients)
            {
                recipeDto.ingredientsPerServing.Add(new IngredientDto { item = item.item, unit = item.unit, value = item.value });
            }

            List<IngredientDto> ingredientList = new List<IngredientDto>();
            foreach (var ing in ingredients)
            {
                ingredientList.Add(new IngredientDto { item = ing.item, unit = ing.unit, value = ing.value });
            }
            recipeDto.ingredientsPerServing = ingredientList;

            var steps = _db.Steps.Where(t => t.recipeId == obj.id).ToList();
            List<StepDto> stepList = new List<StepDto>();
            foreach (var item in steps)
            {
                stepList.Add(new StepDto { stepOrder = item.stepOrder, description = item.description });
            }
            recipeDto.steps = stepList;
            _success.data = recipeDto;
            return Ok(_success);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateUpdateRecipe model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }

            var result = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == id);
            if (result == null)
            {
                _error.message = $"Recipe with id {id} not found";
                return NotFound(_error);
            }

            var category = _unitOfWork.Category.GetFirstOrDefault(t => t.id == model.recipeCategoryId);
            if (category == null)
            {
                _error.message = $"Recipe Category with id {model.recipeCategoryId} not found";
                return BadRequest(_error);
            }

            result.name = model.name;
            result.recipeCategoryId = model.recipeCategoryId;
            result.nServing = model.nServing;
            result.image = model.image;
            result.updatedAt = DateTime.Now;

            _unitOfWork.Recipe.Update(result);
            _unitOfWork.Step.DeleteByRecipeId(result.id);
            _unitOfWork.Ingredient.DeleteByRecipeId(result.id);
            _unitOfWork.Save();

            foreach (var item in model.ingredientsPerServing)
            {
                Ingredient ingredient = new Ingredient
                {
                    recipeId = result.id,
                    item = item.item,
                    unit = item.unit,
                    value = item.value
                };
                _unitOfWork.Ingredient.Add(ingredient);
                _unitOfWork.Save();
            }

            foreach (var item in model.steps)
            {
                Step step = new Step
                {
                    recipeId = result.id,
                    stepOrder = item.stepOrder,
                    description = item.description,
                };
                _unitOfWork.Step.Add(step);
                _unitOfWork.Save();
            }

            UpdateRecipeDto recipeDto = (from recipe in _db.Recipes
                                         select new UpdateRecipeDto()
                                         {
                                             id = recipe.id,
                                             name = recipe.name,
                                             image = recipe.image,
                                             recipeCategoryId = recipe.recipeCategoryId,
                                             nServing = recipe.nServing,
                                             nReactionDislike = recipe.nReactionDislike != null ? recipe.nReactionDislike : 0,
                                             nReactionLike = recipe.nReactionLike != null ? recipe.nReactionLike : 0,
                                             nReactionNeutral = recipe.nReactionNeutral != null ? recipe.nReactionNeutral : 0,
                                             updatedAt = recipe.updatedAt,
                                             createdAt = recipe.createdAt,
                                         }).SingleOrDefault();

            var ingredients = _db.Ingredients.Where(t => t.recipeId == result.id).ToList();
            List<IngredientDto> ingredientList = new List<IngredientDto>();
            foreach (var ing in ingredients)
            {
                ingredientList.Add(new IngredientDto { item = ing.item, unit = ing.unit, value = ing.value });
            }
            recipeDto.ingredientsPerServing = ingredientList;

            var steps = _db.Steps.Where(t => t.recipeId == result.id).ToList();
            List<Step> stepList = new List<Step>();
            foreach (var item in steps)
            {
                stepList.Add(new Step { id = item.id, recipeId = item.recipeId, stepOrder = item.stepOrder, description = item.description });
            }
            recipeDto.steps = stepList;
            _success.data = recipeDto;
            return Ok(_success);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == id);
            if (result == null)
            {
                _error.message = $"Recipe with id {id} not found";
                return NotFound(_error);
            }

            _unitOfWork.Recipe.Remove(result);
            _unitOfWork.Step.DeleteByRecipeId(result.id);
            _unitOfWork.Ingredient.DeleteByRecipeId(result.id);
            _unitOfWork.Save();

            _success.data = new object();
            return Ok(_success);
        }

        [HttpGet]
        public IActionResult GetAll(string q = "", int categoryId = 0, string sort = "name_asc", int limit = 10, int skip = 10)
        {
            List<Recipe> recipes = new List<Recipe>();
            if (categoryId != 0)
            {
                recipes = _db.Recipes.Where(t => t.recipeCategoryId == categoryId).ToList();
            }
            else
            {
                recipes = _db.Recipes.ToList();
            }

            if (!String.IsNullOrEmpty(q))
            {
                recipes = recipes.Where(s => s.name.Contains(q)).ToList();
            }

            switch (sort)
            {
                case "name_desc":
                    recipes = recipes.OrderByDescending(s => s.name).ToList();
                    break;
                case "like_desc":
                    recipes = recipes.OrderByDescending(s => s.nReactionLike).ToList();
                    break;
                default:
                    recipes = recipes.OrderBy(s => s.name).ToList();
                    break;
            }
            int total = recipes.Count;
            GetAllRecipeDto getAllRecipe = new GetAllRecipeDto
            {
                total = recipes.Count
            };
            // recipes = recipes.Skip(skip > total ? 0 : skip).Take(limit > total ? 0 : limit).ToList();
            List<AllRecipe> allRecipes = new List<AllRecipe>();
            foreach (var item in recipes)
            {
                AllRecipe allRecipe = new AllRecipe
                {
                    id = item.id,
                    name = item.name,
                    recipeCategoryId = item.recipeCategoryId,
                    image = item.image,
                    nReactionLike = item.nReactionLike != null ? item.nReactionLike : 0,
                    nReactionNeutral = item.nReactionNeutral != null ? item.nReactionNeutral : 0,
                    nReactionDislike = item.nReactionDislike != null ? item.nReactionDislike : 0,
                    createdAt = item.createdAt,
                    updatedAt = item.updatedAt,
                    recipeCategory = _unitOfWork.Category.GetFirstOrDefault(t => t.id == item.recipeCategoryId)
                };
                allRecipes.Add(allRecipe);
            }
            getAllRecipe.recipes = allRecipes;
            _success.data = getAllRecipe;
            return Ok(_success);

        }

        [HttpGet("{id}")]
        public IActionResult Detail(int id, int nServing = 1)
        {
            if (nServing < 1)
            {
                _error.message = "Target serving minimum 1";
                return BadRequest(_error);
            }
            var recipe = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == id);
            if (recipe == null)
            {
                _error.message = $"Recipe with id {id} not found";
                return NotFound(_error);
            }

            RecipeDetail recipeDetail = new RecipeDetail
            {
                id = recipe.id,
                name = recipe.name,
                image = recipe.image,
                recipeCategoryId = recipe.recipeCategoryId,
                nServing = recipe.nServing,
                nReactionDislike = recipe.nReactionDislike != null ? recipe.nReactionDislike : 0,
                nReactionLike = recipe.nReactionLike != null ? recipe.nReactionLike : 0,
                nReactionNeutral = recipe.nReactionNeutral != null ? recipe.nReactionNeutral : 0,
                updatedAt = recipe.updatedAt,
                createdAt = recipe.createdAt
            };
            recipeDetail.recipeCategory = _unitOfWork.Category.GetFirstOrDefault(t => t.id == recipeDetail.recipeCategoryId);

            var ingredients = _db.Ingredients.Where(t => t.recipeId == recipeDetail.id).ToList();
            List<IngredientDto> ingredientList = new List<IngredientDto>();
            foreach (var ing in ingredients)
            {
                ingredientList.Add(new IngredientDto { item = ing.item, unit = ing.unit, value = ing.value });
            }
            recipeDetail.ingredientsPerServing = ingredientList;

            _success.data = recipeDetail;
            return Ok(_success);
        }

        [HttpGet("{id}/steps")]
        public IActionResult Step(int id)
        {
            List<StepDto> recipeDto = new List<StepDto>();

            var recipe = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == id);
            if (recipe == null)
            {
                _error.message = $"Recipe with id {id} not found";
                return NotFound(_error);
            }

            var steps = _db.Steps.Where(t => t.recipeId == recipe.id).ToList();
            List<StepDto> stepList = new List<StepDto>();
            foreach (var item in steps)
            {
                stepList.Add(new StepDto { stepOrder = item.stepOrder, description = item.description });
            }
            recipeDto = stepList;

            _success.data = recipeDto;
            return Ok(_success);
        }
    }
}
