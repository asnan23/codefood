using CodeFood_API.Asnan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository.IRepository
{
    public interface IStepRepository : IRepository<Step>
    {
        void Update(Step obj);
        void DeleteByRecipeId(int id);
    }
}
