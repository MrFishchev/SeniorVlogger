using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
