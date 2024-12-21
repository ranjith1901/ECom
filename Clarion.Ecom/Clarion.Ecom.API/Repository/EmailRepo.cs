using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.NonEntity;
using Microsoft.EntityFrameworkCore;
using Clarion.Ecom.API.Models;

namespace Clarion.Ecom.API.Repository
{
    public class EmailRepo : IEmail
    {

        /// <summary>
        /// DBContext
        /// </summary>
        private ClarionECOMDBContext _context;
        /// <summary>
        /// Current Http ContextAccessor
        /// </summary>
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="emailConfig"></param>
        /// <param name="context"></param>
        /// <param name="contextAccessor"></param>
        public EmailRepo(ClarionECOMDBContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public void Send(MessageModel message)
        {
            //var emailAccount = _context.EmailAccounts.FirstOrDefault(); // Assuming you want to get the first email account

            //if (emailAccount != null)
            //{
            //    var emailConfig = new EmailConfigModel
            //    {
            //        From = emailAccount.DisplayName ?? emailAccount.Email,
            //        SmtpServer = emailAccount.Host,
            //        Port = emailAccount.Port,
            //        UserName = emailAccount.Username,
            //        Password = emailAccount.Password
            //    };

            //    var emailMessage = CreateEmailMessage(message, emailConfig);
            //    SendEmail(emailMessage, emailConfig);
            //}
            //else
            //{
            //    throw new Exception("No email account found in the database.");
            //}
        }


        public void SendEmail(MimeMessage emailMessage, EmailConfigModel emailConfig)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
                client.Authenticate(emailConfig.UserName, emailConfig.Password);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }

        public MimeMessage CreateEmailMessage(MessageModel message, EmailConfigModel emailConfig)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Your Name", emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };

            return emailMessage;
        }






    }
}
