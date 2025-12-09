using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class UserService
    {
        public readonly UserRepository _userRepository; 
        private readonly ApplicationDbContext _context;

        public UserService(UserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
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
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
