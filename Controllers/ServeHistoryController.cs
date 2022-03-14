using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Param;
using CodeFood_API.Asnan.Models.Response;
using CodeFood_API.Asnan.Repository;
using CodeFood_API.Asnan.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Controllers
{
    //  [Authorize]
    [AllowAnonymous]
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
            if (recipe == null)
            {
                _error.message = $"Recipe with id {model.recipeId} not found";
                return NotFound(_error);
            }

            var step = _db.Steps.Where(t => t.recipeId == recipe.id);
            var category = _db.Categories.Where(t => t.id == recipe.recipeCategoryId).FirstOrDefault();

            // var number = _utility.GenerateNumber();
            var serveHistory = new ServeHistory
            {
                code = _utility.GenerateNumber(),
                userId = Convert.ToInt32(User.FindFirst("UserId")?.Value),
                nServing = model.nServing,
                recipeId = model.recipeId,
                nStep = step.Count(),
                nStepDone = 1,
                status = "progress",
            };
            _unitOfWork.ServeHistory.Add(serveHistory);
            _unitOfWork.Save();

            var serve = _unitOfWork.ServeHistory.GetbyCode(serveHistory.code);
            StartCookingDto result = new StartCookingDto
            {
                id = serve.code,
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
            foreach (var item in step.OrderBy(t => t.stepOrder))
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

            var serve = _unitOfWork.ServeHistory.GetbyCode(id);
            if (serve == null)
            {
                _error.message = $"Serve history with id {id} not found";
                return NotFound(_error);
            }

            if ((serve.nStepDone + 1) < model.stepOrder)
            {
                _error.message = $"Some steps before {model.stepOrder} is not done yet";
                return StatusCode(409, _error);
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
                id = serve.code,
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
            foreach (var item in step.OrderBy(t => t.stepOrder))
            {
                cookingSteps.Add(new cookingStep { stepOrder = item.stepOrder, description = item.description, done = item.stepOrder <= serve.nStepDone ? true : false });
            }
            result.steps = cookingSteps;
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
            var user = User.FindFirst("UserId")?.Value;
            var serve = _unitOfWork.ServeHistory.GetbyCode(id);
            if (serve == null)
            {
                _error.message = $"Serve history with id {id} not found";
                return NotFound(_error);
            }

            if (serve.status != "need-rating")
            {
                _error.message = $"Invalid status, status need to be need-reaction";
                return BadRequest(_error);
            }

            if (serve.userId != Convert.ToInt32(User.FindFirst("UserId")?.Value))
            {
                _error.message = $"Forbidden";
                return StatusCode(403, _error);
            }
            serve.reaction = model.reaction;
            _unitOfWork.ServeHistory.Update(serve);
            _unitOfWork.Save();

            var recipe = _unitOfWork.Recipe.GetFirstOrDefault(t => t.id == serve.recipeId);
            var step = _db.Steps.Where(t => t.recipeId == recipe.id);
            var category = _db.Categories.Where(t => t.id == recipe.recipeCategoryId).FirstOrDefault();

            StartCookingDto result = new StartCookingDto
            {
                id = serve.code,
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
            foreach (var item in step.OrderBy(t => t.stepOrder))
            {
                cookingSteps.Add(new cookingStep { stepOrder = item.stepOrder, description = item.description, done = item.stepOrder <= serve.nStepDone ? true : false });
            }
            result.steps = cookingSteps;

            _success.data = result;
            return Ok(_success);
        }

        [HttpGet("{id}")]
        public IActionResult Detail(string id)
        {
            var serve = _unitOfWork.ServeHistory.GetbyCode(id);
            if (serve == null)
            {
                _error.message = $"Serve history with id {id} not found";
                return NotFound(_error);
            }

            var step = _db.Steps.Where(t => t.recipeId == serve.recipeId);
            var recipe = _db.Recipes.Where(t => t.id == serve.recipeId).FirstOrDefault();
            var category = _db.Categories.Where(t => t.id == recipe.recipeCategoryId).FirstOrDefault();

            StartCookingDto result = new StartCookingDto
            {
                id = serve.code,
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
            foreach (var item in step.OrderBy(t => t.stepOrder))
            {
                cookingSteps.Add(new cookingStep { stepOrder = item.stepOrder, description = item.description, done = item.stepOrder <= serve.nStepDone ? true : false });
            }
            result.steps = cookingSteps;
            _success.data = result;
            return Ok(_success);
        }


        [HttpGet]
        public IActionResult GetAll(string q = "", int categoryId = 0, string sort = "", int limit = 10, int skip = 0, string status = "")
        {
            var serveHistories = (from serve in _db.ServeHistories
                                  join recipe in _db.Recipes on serve.recipeId equals recipe.id
                                  join category in _db.Categories on recipe.recipeCategoryId equals category.id
                                  select new { serve, recipe,category }).ToList();
            if (categoryId != 0) { serveHistories = serveHistories.Where(t => t.recipe.recipeCategoryId == categoryId).ToList(); }
            if (!string.IsNullOrEmpty(status)) { serveHistories = serveHistories.Where(t => t.serve.status == status).ToList(); }
            if (!string.IsNullOrEmpty(q)) { serveHistories = serveHistories.Where(t => t.recipe.name.Contains(q)).ToList(); }
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "oldest":
                        serveHistories = serveHistories.OrderByDescending(s => s.serve.updatedAt).ToList();
                        break;
                    case "nserve_asc":
                        serveHistories = serveHistories.OrderBy(s => s.serve.nServing).ToList();
                        break;
                    case "nserve_desc":
                        serveHistories = serveHistories.OrderByDescending(s => s.serve.nServing).ToList();
                        break;
                    default:
                        serveHistories = serveHistories.OrderByDescending(s => s.serve.updatedAt).ToList();
                        break;
                }
            }

            ServeHisotryResponse getAllHistory = new ServeHisotryResponse
            {
                total = serveHistories.Count
            };
            List<ServeHistoryAllDto> serveHistoryAlls = new List<ServeHistoryAllDto>();
            foreach (var item in serveHistories)
            {
                ServeHistoryAllDto history = new ServeHistoryAllDto
                {
                    id = item.serve.code,
                    userId = item.serve.userId,
                    nServing = item.serve.nServing,
                    recipeId = item.serve.recipeId,
                    recipeCategoryId = item.category.id,
                    recipeCategoryName = item.category.name,
                    recipeName = item.recipe.name,
                    recipeImage = item.recipe.image,
                    reaction = item.serve.reaction,
                    nStep = item.serve.nStep,
                    nStepDone = item.serve.nStepDone,
                    status = item.serve.status,
                    createdAt = item.serve.createdAt,
                    updatedAt = item.serve.updatedAt
                };
                serveHistoryAlls.Add(history);
            }
            getAllHistory.history = serveHistoryAlls;
            _success.data = getAllHistory;
            // recipes = recipes.Skip(skip > total ? 0 : skip).Take(limit > total ? 0 : limit).ToList();

            return Ok(_success);

        }
    }
}
