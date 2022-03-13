using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Dto;
using CodeFood_API.Asnan.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public class MasterUserRepository : Repository<MasterUser>, IMasterUserRepository
    {
        private ApplicationDbContext _db;

        public MasterUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MasterUser obj)
        {
            _db.MasterUsers.Update(obj);
        }

        
    }
}
