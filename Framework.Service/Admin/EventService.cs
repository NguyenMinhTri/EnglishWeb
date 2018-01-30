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
    public interface IEventService : IQlService<Event>
    {
        bool IsFreeTime( string inCurrentUser);
    }
    public class EventService : QlService<Event>, IEventService
    {
        IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository, IUnitOfWork unitOfWork) : base(eventRepository, unitOfWork) 
        {
            this._repository = eventRepository;
            _eventRepository = eventRepository;
        }

        public bool IsFreeTime(string inCurrentUser)
        {
            DateTime inTimeNow = DateTime.Now.AddHours(7);
            var listFreeTimeInDay = _eventRepository.GetMulti(time => time.eventstart.Day == inTimeNow.Day && time.userID == inCurrentUser);
            foreach(var SubTime  in listFreeTimeInDay)
            {
                if(inTimeNow.Ticks >= SubTime.eventstart.Ticks && inTimeNow.Ticks  <= SubTime.eventend.Ticks)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
