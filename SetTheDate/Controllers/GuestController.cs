using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;
using SetTheDate.ModelFactories;

namespace SetTheDate.Controllers
{
    public class GuestController
    {
        private readonly GuestModelFactory _guestModelFactory;

        public GuestController(GuestModelFactory guestModelFactory)
        {
            _guestModelFactory = guestModelFactory;
        }

    }
}
