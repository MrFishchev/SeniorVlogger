﻿using System;
using System.Threading.Tasks;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUsers { get; }
        IBlogPostRepository BlogPosts { get; }
        IBlogFileRepository BlogFiles { get; }
        ICategoryRepository Categories { get; }
        ISubscriptionRepository Subscriptions { get; }

        Task Save();
    }
}
