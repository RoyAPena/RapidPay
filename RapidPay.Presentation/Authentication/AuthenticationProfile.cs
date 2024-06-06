using AutoMapper;
using RapidPay.Application.Authentication.Command.Login;
using RapidPay.Presentation.Authentication.Dtos.Request;

namespace RapidPay.Presentation.Authentication
{
    internal class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<LoginDto, LoginCommand>();
        }
    }
}