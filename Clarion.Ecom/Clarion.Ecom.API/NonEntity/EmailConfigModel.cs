namespace Clarion.Ecom.API.NonEntity
{
    public class EmailConfigModel
    {

        public string From { get; set; } = null!;

        public string SmtpServer { get; set; } = null!;

        public int Port { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;


    }
}
