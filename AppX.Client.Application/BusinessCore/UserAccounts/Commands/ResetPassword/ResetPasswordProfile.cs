using AppX.Client.Domain.Entities.UserAccount;
using AutoMapper;

namespace AppX.Client.Application.BusinessCore.UserAccounts.Commands.ResetPassword
{
    public class ResetPasswordProfile : Profile
    {
        public ResetPasswordProfile()
        {
            CreateMap<ResetPasswordCommand, ResetPasswordDto>().ReverseMap();
        }
    }
}
