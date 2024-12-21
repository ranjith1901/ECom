using MimeKit;
using Clarion.Ecom.API.NonEntity;

namespace Clarion.Ecom.API.IRepository
{
    public interface IEmail
    {
        void Send(MessageModel message);
        MimeMessage CreateEmailMessage(MessageModel message, EmailConfigModel emailConfig);
        void SendEmail(MimeMessage emailMessage, EmailConfigModel emailConfig);

    }
}
