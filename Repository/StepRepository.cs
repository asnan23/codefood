using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public class StepRepository : Repository<Step>, IStepRepository
    {
        private ApplicationDbContext _db;

        public StepRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Step obj)
        {
            _db.Steps.Update(obj);
        }

        public void DeleteByRecipeId(int recipeId)
        {
            var steps = _db.Steps.Where(t => t.recipeId == recipeId);
            foreach (var item in steps)
            {
                _db.Steps.Remove(item);
            }
            
        }
    }
}
