using CodeFood_API.Asnan.Models;
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
    [AllowAnonymous]
    [Route("/recipe-categories")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        protected SuccessDTO _success;
        protected ErrorDTO _error;
        private readonly IValidationErrorRepository _validationErrorRepository;
        public CategoryController(IUnitOfWork unitOfWork, IValidationErrorRepository validationErrorRepository)
        {
            _unitOfWork = unitOfWork;
            this._success = new SuccessDTO();
            this._error = new ErrorDTO();
            _validationErrorRepository = validationErrorRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _unitOfWork.Category.GetAll();
            _success.data = categories;
            return Ok(_success);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }

            Category obj = new Category{name = model.name};
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();

            _success.data = obj;
            return Ok(_success);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }
            var category = _unitOfWork.Category.GetFirstOrDefault(t => t.id == id);
            if(category == null)
            {
                _error.message = $"Recipe Category with id {id} not found";
                return NotFound(_error);
            }
            category.name = model.name;
            category.updatedAt = DateTime.Now;
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();

            _success.data = category;
            return Ok(_success);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                var validasi = _validationErrorRepository.Validate(ModelState);
                _error.message = validasi;
                return BadRequest(_error);
            }
            var category = _unitOfWork.Category.GetFirstOrDefault(t => t.id == id);
            if (category == null)
            {
                _error.message = $"Recipe Category with id {id} not found";
                return NotFound(_error);
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();

            _success.data = new object();
            return Ok(_success);
        }
    }
}
