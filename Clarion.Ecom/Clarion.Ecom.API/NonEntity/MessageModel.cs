using MimeKit;

namespace Clarion.Ecom.API.NonEntity
{
    public class MessageModel
    {


        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MessageModel()
        {

        }
        public MessageModel(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            // Converting email addresses to MailboxAddress and adding them to the 'To' list
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));

            // Setting the subject and content of the message
            Subject = subject;
            Content = content;

        }

    }
}
