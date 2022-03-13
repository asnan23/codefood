using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Param;
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
    [Route("/serve-histories")]
    public class ServeHistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        protected SuccessDTO _success;
        protected ErrorDTO _error;
        private readonly ApplicationDbContext _db;
        private readonly IValidationErrorRepository _validationErrorRepository;
        private readonly IUtility _utility;
        public ServeHistoryController(IUnitOfWork unitOfWork, ApplicationDbContext db, 
            IValidationErrorRepository validationErrorRepository, IUtility utility)
        {
            _unitOfWork = unitOfWork;
            this._success = new SuccessDTO();
            this._error = new ErrorDTO();
            _db = db;
            _validationErrorRepository = validationErrorRepository;
            _utility = utility;
        }

        [HttpPost]
        public IActionResult Post([FromBody] StartCooking model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }

            var recipe = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == model.recipeId);
            if(recipe == null)
            {
                _error.message = $"Recipe with id {model.recipeId} not found";
                return NotFound(_error);
            }

            var step = _db.Steps.Where(t => t.recipeId == recipe.id);
            var category = _db.Categories.Where(t => t.id == recipe.recipeCategoryId).FirstOrDefault();

           // var number = _utility.GenerateNumber();
            var serveHistory = new ServeHistory
            {
                id = "ABC",
                userId = Convert.ToInt32(User.FindFirst("UserId")?.Value),
                nServing = model.nServing,
                recipeId = model.recipeId,
                nStep = step.Count(),
                nStepDone = 1,
                status = "progress",
            };
            _unitOfWork.ServeHistory.Add(serveHistory);
            _unitOfWork.Save();

            var serve = _unitOfWork.ServeHistory.GetFirstOrDefault(t => t.id == serveHistory.id);
            StartCookingDto result = new StartCookingDto
            {
                id = serve.id,
                userId = serve.userId,
                nServing = serve.nServing,
                recipeId = serve.recipeId,
                recipeCategoryId = category.id,
                recipeCategoryName = category.name,
                recipeName = recipe.name,
                recipeImage = recipe.image,
                reaction = serve.reaction,
                nStep = serve.nStep,
                nStepDone = serve.nStepDone,
                status = serve.status,
                createdAt = serve.createdAt,
                updatedAt = serve.updatedAt
            };
            List<cookingStep> cookingSteps = new List<cookingStep>();
            foreach (var item in step.OrderBy(t=>t.stepOrder))
            {
                cookingSteps.Add(new cookingStep { stepOrder = item.stepOrder, description = item.description, done = item.stepOrder <= serve.nStepDone ? true : false });
            }
            result.steps = cookingSteps;
            _success.data = result;
            return Ok(_success);
        }

        [HttpPut("{id}/done-step")]
        public IActionResult Put(string id, [FromBody] UpdateStep model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }

            var serve = _unitOfWork.ServeHistory.GetFirstOrDefault(t => t.id == id);
            if (serve == null)
            {
                _error.message = $"Serve history with id {id} not found";
                return NotFound(_error);
            }

            if((serve.nStepDone + 1) > model.stepOrder)
            {
                _error.message = $"Some steps before {model.stepOrder} is not done yet";
                return StatusCode(409,_error);
            }

            if (serve.userId != Convert.ToInt32(User.FindFirst("UserId")?.Value))
            {
                _error.message = $"Forbidden";
                return StatusCode(403, _error);
            }

            serve.nStepDone = model.stepOrder;
            _unitOfWork.ServeHistory.Update(serve);
            _unitOfWork.Save();

            var recipe = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == serve.recipeId);
            var step = _db.Steps.Where(t => t.recipeId == recipe.id);
            var category = _db.Categories.Where(t => t.id == recipe.recipeCategoryId).FirstOrDefault();

            StartCookingDto result = new StartCookingDto
            {
                id = serve.id,
                userId = serve.userId,
                nServing = serve.nServing,
                recipeId = serve.recipeId,
                recipeCategoryId = category.id,
                recipeCategoryName = category.name,
                recipeName = recipe.name,
                recipeImage = recipe.image,
                reaction = serve.reaction,
                nStep = serve.nStep,
                nStepDone = serve.nStepDone,
                status = serve.status,
                createdAt = serve.createdAt,
                updatedAt = serve.updatedAt
            };
            foreach (var item in step.OrderBy(t => t.stepOrder))
            {
                result.steps.Add(new cookingStep { stepOrder = item.stepOrder, description = item.description, done = item.stepOrder <= serve.nStepDone ? true : false });
            }

            _success.data = result;
            return Ok(_success);
        }

        [HttpPost("{id}/reaction")]
        public IActionResult Reaction(string id, [FromBody] Reaction model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }

            var serve = _unitOfWork.ServeHistory.GetFirstOrDefault(t => t.id == id);
            if (serve == null)
            {
                _error.message = $"Serve history with id {id} not found";
                return NotFound(_error);
            }

            if (serve.reaction != null || serve.reaction != "need-rating")
            {
                _error.message = $"Invalid status, status need to be need-reaction";
                return BadRequest(_error);
            }
            if(serve.userId != Convert.ToInt32(User.FindFirst("UserId")?.Value))
            {
                _error.message = $"Forbidden";
                return StatusCode(403,_error);
            }
            serve.reaction = model.reaction;
            _unitOfWork.ServeHistory.Update(serve);
            _unitOfWork.Save();

            var recipe = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == serve.recipeId);
            var step = _db.Steps.Where(t => t.recipeId == recipe.id);
            var category = _db.Categories.Where(t => t.id == recipe.recipeCategoryId).FirstOrDefault();

            StartCookingDto result = new StartCookingDto
            {
                id = serve.id,
                userId = serve.userId,
                nServing = serve.nServing,
                recipeId = serve.recipeId,
                recipeCategoryId = category.id,
                recipeCategoryName = category.name,
                recipeName = recipe.name,
                recipeImage = recipe.image,
                reaction = serve.reaction,
                nStep = serve.nStep,
                nStepDone = serve.nStepDone,
                status = serve.status,
                createdAt = serve.createdAt,
                updatedAt = serve.updatedAt
            };
            foreach (var item in step.OrderBy(t => t.stepOrder))
            {
                result.steps.Add(new cookingStep { stepOrder = item.stepOrder, description = item.description, done = item.stepOrder <= serve.nStepDone ? true : false });
            }

            _success.data = result;
            return Ok(_success);
        }
    }
}
