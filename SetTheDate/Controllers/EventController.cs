using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;
using SetTheDate.ModelFactories;

namespace SetTheDate.Controllers
{
    public class EventController
    {
        private readonly EventModelFactory _eventModelFactory;

        public EventController(EventModelFactory eventModelFactory)
        {
            _eventModelFactory = eventModelFactory;
        }
    }
}
