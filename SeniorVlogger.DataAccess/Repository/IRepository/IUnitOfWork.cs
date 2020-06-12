using System;
using System.Threading.Tasks;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUsers { get; }
        IBlogPostRepository BlogPosts { get; }
        IBlogFileRepository BlogFiles { get; }

        Task Save();
    }
}
