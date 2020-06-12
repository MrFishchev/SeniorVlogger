﻿using System.Threading.Tasks;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IApplicationUserRepository ApplicationUsers { get; }
        public IBlogPostRepository BlogPosts { get; }
        public IBlogFileRepository BlogFiles { get; set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUsers = new ApplicationUserRepository(db);
            BlogPosts = new BlogPostRepository(db);
            BlogFiles = new BlogFileRepository(db);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
