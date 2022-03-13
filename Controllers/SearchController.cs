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
    [Route("/search")]
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        protected SuccessDTO _success;
        protected ErrorDTO _error;
        private readonly ApplicationDbContext _db;
        public SearchController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            this._success = new SuccessDTO();
            this._error = new ErrorDTO();
            _db = db;
        }

        [HttpGet("recipes")]
        public IActionResult GetAll(int limit = 5, string q = "")
        {
            var recipes = _db.Recipes.Where(t => t.name.Contains(q));
            List<object> list = new List<object>();
            foreach (var item in recipes)
            {
                list.Add(new { id = item.id, name = item.name });
            }
            _success.data = list;
            return Ok(_success);
        }

    }
}
