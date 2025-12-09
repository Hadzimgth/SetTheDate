using AutoMapper;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Models;

namespace SetTheDate.Libraries
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<ContactInformation, ContactInformationModel>().ReverseMap();
            CreateMap<ContactInformationModel, ContactInformation>().ReverseMap();
            CreateMap<EventAnswer, EventAnswerModel>().ReverseMap();
            CreateMap<EventAnswerModel, EventAnswer>().ReverseMap();
            CreateMap<EventGuest, EventGuestModel>().ReverseMap();
            CreateMap<EventGuestModel, EventGuest>().ReverseMap();
            CreateMap<EventGuestAnswer, EventGuestAnswerModel>().ReverseMap();
            CreateMap<EventGuestAnswerModel, EventGuestAnswer>().ReverseMap();
            CreateMap<EventImageAttachment, EventImageAttachmentModel>().ReverseMap();
            CreateMap<EventImageAttachmentModel, EventImageAttachment>().ReverseMap();
            CreateMap<EventQuestion, EventQuestionModel>().ReverseMap();
            CreateMap<EventQuestionModel, EventQuestion>().ReverseMap();
            CreateMap<GuestWishes, GuestWishesModel>().ReverseMap();
            CreateMap<GuestWishesModel, GuestWishes>().ReverseMap();
            CreateMap<PaymentInformation, PaymentInformationModel>().ReverseMap();
            CreateMap<PaymentInformationModel, PaymentInformation>().ReverseMap();
            CreateMap<Setting, SettingModel>().ReverseMap();
            CreateMap<SettingModel, Setting>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<User, RegisterModel>().ReverseMap();
            CreateMap<RegisterModel, User>().ReverseMap();
            CreateMap<UserEvent, UserEventModel>().ReverseMap();
            CreateMap<UserEventModel, UserEvent>().ReverseMap();
            CreateMap<WeddingCardInformation, UserEventModel>().ReverseMap();
            CreateMap<UserEventModel, WeddingCardInformation>().ReverseMap();
            CreateMap<WeddingCardInformation, WeddingCardInformationModel>().ReverseMap();
            CreateMap<WeddingCardInformationModel, WeddingCardInformation>().ReverseMap();


        }
    }
}
