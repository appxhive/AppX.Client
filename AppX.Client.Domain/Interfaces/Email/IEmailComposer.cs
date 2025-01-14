using AppX.Client.Domain.Entities.Common;

namespace AppX.Client.Domain.Interfaces.Email
{
    public interface IEmailComposer
    {
        EmailObject Compose(
            string from,
            string recipient,
            string cc,
            string subject,
            string body,
            string attachedFile,
            string request);
    }
}
