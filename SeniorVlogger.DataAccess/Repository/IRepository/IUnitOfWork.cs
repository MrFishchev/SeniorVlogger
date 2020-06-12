using System;
using System.Threading.Tasks;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }

        Task Save();
    }
}
