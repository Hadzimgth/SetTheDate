using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class EventService
    {
        public readonly UserEventRepository _userEventRepository;
        public readonly EventQuestionRepository _eventQuestionRepository;
        public readonly EventGuestRepository _eventGuestRepository;
        public readonly EventGuestAnswerRepository _eventGuestAnswerRepository;
        public readonly EventAnswerRepository _eventAnswerRepository;

        public EventService(UserEventRepository userEventRepository)
        {
            _userEventRepository = userEventRepository;
        }

        public async Task<UserEvent> GetEntityById(int id)
        {
            return await _userEventRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserEvent>> GetAllEntities()
        {
            return await _userEventRepository.GetAllAsync();
        }
    }
}
