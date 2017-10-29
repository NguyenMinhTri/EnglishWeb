using Framework.FrameworkContext;
using System;

namespace Framework.Repository.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        FrameworkDbContext Init();
    }
}