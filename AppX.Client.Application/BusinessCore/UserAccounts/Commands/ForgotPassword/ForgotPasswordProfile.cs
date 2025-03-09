using AppX.Client.Domain.Entities.UserAccount;
using AutoMapper;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.ForgotPassword
{
    public class ForgotPasswordProfile : Profile
    {
        public ForgotPasswordProfile()
        {
            CreateMap<ForgotPasswordCommand, ForgotPasswordDto>().ReverseMap();
        }
    }
}
