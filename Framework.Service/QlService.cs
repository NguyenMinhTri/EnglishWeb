using Framework.Model;
using Framework.Repository.Infrastructure;
using Framework.Repository.RepositorySpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service
{
    public interface IQlService<T> where T : class
    {
        List<T> GetAll();
        T Add(T entity);
        T GetById(int id);
        T Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        void Save();
    }
    public class QlService<T> : IQlService<T> where T : class, IAuditable
    {
        protected IRepository<T> _repository;
        protected IUnitOfWork _unitOfWork;
        public QlService(IRepository<T> repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public virtual List<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public virtual T Add(T entity)
        {
            entity.Status = true;
            entity.CreatedDate = DateTime.Now;
            return _repository.Add(entity);
        }

        public virtual T Update(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _repository.Update(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public virtual void Delete(int id)
        {
            Delete(GetById(id));
        }

        public virtual void Save()
        {
            _unitOfWork.Commit();
        }


        public T GetById(int id)
        {
            return _repository.GetSingleById(id);
        }
    }
}
