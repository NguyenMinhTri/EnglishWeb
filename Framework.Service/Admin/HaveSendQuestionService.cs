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
    public interface IHaveSendQuestionService : IQlService<HaveSendQuestion>
    {
    }
    public class HaveSendQuestionService : QlService<HaveSendQuestion>, IHaveSendQuestionService
    {
        IHaveSendQuestionRepository _haveSendQuestionRepository;
        public HaveSendQuestionService(IHaveSendQuestionRepository haveSendQuestionRepository, IUnitOfWork unitOfWork) : base(haveSendQuestionRepository, unitOfWork) 
        {
            this._repository = haveSendQuestionRepository;
        }
    }
}
