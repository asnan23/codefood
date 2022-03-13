using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        private ApplicationDbContext _db;

        public IngredientRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Ingredient obj)
        {
            _db.Ingredients.Update(obj);
        }

        public void DeleteByRecipeId(int recipeId)
        {
            var ingredients = _db.Ingredients.Where(t => t.recipeId == recipeId);
            foreach (var item in ingredients)
            {
                _db.Ingredients.Remove(item);
            }

        }
    }
}
