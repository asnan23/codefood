using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Recipe = new RecipeRepository(_db);
            Step = new StepRepository(_db);
            Ingredient = new IngredientRepository(_db);
            ServeHistory = new ServeHistoryRepository(_db);
            MasterUser = new MasterUserRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public IRecipeRepository Recipe { get; private set; }
        public IIngredientRepository Ingredient { get; private set; }
        public IStepRepository Step { get; private set; }
        public IServeHistoryRepository ServeHistory { get; private set; }
        public IMasterUserRepository MasterUser { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
