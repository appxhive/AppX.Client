using AppX.Client.Domain.Entities.Common;
using AppX.Client.Domain.Interfaces.Email;

namespace AppX.Client.Infrastructure.Services.Email
{
    public class EmailComposer : IEmailComposer
    {
        public EmailObject Compose(
            string from,
            string recipient,
            string cc,
            string subject,
            string body,
            string attachedFile,
            string request)
        {
            return new EmailObject
            (
                $"{from}",
                $"{recipient}",
                $"{cc}",
                $"{subject}",
                $"{body}",
                $"{attachedFile}",
                $"{request}"
            );
        }
    }
}
