using AppX.Client.Domain.Entities.UserAccount;
using AutoMapper;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.LoginUser
{
    public class LogInUserProfile : Profile
    {
        public LogInUserProfile()
        {
            CreateMap<LoginUserCommand, LoginUserModel>();
        }
    }
}
