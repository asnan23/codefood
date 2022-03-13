using CodeFood_API.Asnan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.DatabaseInitializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
