namespace Framework.Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}