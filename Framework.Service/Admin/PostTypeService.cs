using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Service.Admin
{
    public interface IPostTypeService : IQlService<PostType>
    {
    }
    public class PostTypeService : QlService<PostType>, IPostTypeService
    {
        IPostTypeRepository _postTypeRepository;
        public PostTypeService(IPostTypeRepository postTypeRepository, IUnitOfWork unitOfWork) : base(postTypeRepository, unitOfWork) 
        {
            this._repository = postTypeRepository;
        }
    }
}
