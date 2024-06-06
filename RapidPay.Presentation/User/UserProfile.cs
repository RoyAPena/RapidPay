using AutoMapper;
using RapidPay.Application.Users.Command.CreateUser;
using RapidPay.Presentation.User.Dtos.Request;

namespace RapidPay.Presentation.User
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, CreateUserCommand>();
        }
    }
}