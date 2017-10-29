using Framework.FrameworkContext;
namespace Framework.Repository.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private FrameworkDbContext dbContext;

        public FrameworkDbContext Init()
        {
            return dbContext ?? (dbContext = new FrameworkDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}