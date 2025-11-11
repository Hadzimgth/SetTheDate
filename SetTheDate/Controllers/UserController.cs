using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;
using SetTheDate.ModelFactories;

namespace SetTheDate.Controllers
{
    public class UserController
    {
        private readonly UserModelFactory _userModelFactory;

        public UserController(UserModelFactory userModelFactory)
        {
            _userModelFactory = userModelFactory;
        }

    }
}
