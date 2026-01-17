using AutoMapper;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class GuestModelFactory
    {
        private readonly GuestService _guestService;
        private readonly AttachmentService _attachmentService;
        private readonly IMapper _mapper;

        public GuestModelFactory(GuestService guestService,
            AttachmentService attachmentService, IMapper mapper)
        {
            _guestService = guestService;
            _attachmentService = attachmentService;
            _mapper = mapper;
        }

        public async Task<WeddingCardInformationModel> PrepareWeddingCardByEventIdAsync(int eventId)
        {
            var model = await _guestService.GetWeddingCardByEventIdAsync(eventId);
            var weddingCard =  _mapper.Map<WeddingCardInformationModel>(model);

            var contactInformations = await _guestService.GetContactInformationListByWeddingCardInformationId(weddingCard.Id);
            var eventImages = await _attachmentService.GetImagesByEventWeddingCardIdAsync(weddingCard.Id);

            weddingCard.ContactInformations = _mapper.Map<List<ContactInformationModel>>(contactInformations);
            weddingCard.EventImages = _mapper.Map<List<EventImageAttachmentModel>>(eventImages);

            return weddingCard;
        }

        public async Task InsertGuestWishes(GuestWishesModel guestWishesModel)
        {
            var entity = _mapper.Map<GuestWishes>(guestWishesModel);
            await _guestService.InsertWishes(entity);
        }
    }
}
