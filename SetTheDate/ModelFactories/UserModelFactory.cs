using AutoMapper;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class UserModelFactory
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserModelFactory(UserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserModel> GetEntityByIdAsync(int id)
        {
            var entity = await _userService.GetEntityById(id);
            var model = _mapper.Map<UserModel>(entity);
            return model;
        }

        public async Task<IEnumerable<UserModel>> GetAllEntitiesAsync()
        {
            var entities = await _userService.GetAllEntities();
            var models = _mapper.Map<List<UserModel>>(entities);
            return models;
        }
    }
}
