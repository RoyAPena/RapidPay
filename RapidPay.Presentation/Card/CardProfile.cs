using AutoMapper;
using RapidPay.Application.Cards.Command.CreateCard;
using RapidPay.Application.Cards.Command.Pay;
using RapidPay.Presentation.Card.Dtos.Request;

namespace RapidPay.Presentation.Card
{
    internal class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CreateCardDto, CreateCardCommand>();
            CreateMap<PayDto, PayCommand>();
        }
    }
}