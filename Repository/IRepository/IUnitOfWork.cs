using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IRecipeRepository Recipe { get; }
        IIngredientRepository Ingredient { get; }
        IStepRepository Step { get; }
        IServeHistoryRepository ServeHistory { get; }
        IMasterUserRepository MasterUser { get; }
        void Save();
    }
}
