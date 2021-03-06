using CodeFood_API.Asnan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository.IRepository
{
    public interface IServeHistoryRepository : IRepository<ServeHistory>
    {
        void Update(ServeHistory obj);
        ServeHistory GetbyCode(string code);
    }
}
