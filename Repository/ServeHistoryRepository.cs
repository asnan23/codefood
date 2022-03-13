using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Dto;
using CodeFood_API.Asnan.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public class ServeHistoryRepository : Repository<ServeHistory>, IServeHistoryRepository
    {
        private ApplicationDbContext _db;

        public ServeHistoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ServeHistory obj)
        {
            _db.ServeHistories.Update(obj);
        }

        
    }
}
