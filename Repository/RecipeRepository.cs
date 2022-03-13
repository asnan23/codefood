using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Dto;
using CodeFood_API.Asnan.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        private ApplicationDbContext _db;

        public RecipeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Recipe obj)
        {
            _db.Recipes.Update(obj);
        }

        
    }
}
