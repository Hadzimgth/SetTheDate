using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class UserService
    {
        public readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<User> InsertUser(User user)
        {
            _userRepository.Add(user);

            return (await _userRepository.GetAllAsync()).Where(x => x.Email == user.Email).FirstOrDefault();
        }
    }
}
