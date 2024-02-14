using MimeKit;
using MailKit.Net.Smtp;
using System.Web.Helpers;
using Telegram.Bot.Types;
namespace BrickTrickWeb
{
    
    internal class ServiceSendEmailUser
    {
        private List<(int port, string smtServer, bool UseTls)> valuesToEmail = new List<(int port, string smtServer, bool UseTls)>
        {
            (587, "smtp.gmail.com", false), // gmail version
            (465, "smtp.mail.ru",true) // mail
        };
        public string? Message { get; init; }
        public string Email { get; init; }
        public string? Subject { get; init; }
        public string Name {  get; init; }
        public ServiceSendEmailUser(string email, string message, string name, string? subject = null)
        {
            Message = message;
            Email = email;
            Name = name;
            Subject = subject;
        }
        public async void SendLetter( string signature = "Администрация сайта")
        {
            MimeMessage emailMessage = new MimeMessage();
            // From от кого а To для кого
            (int port, string smtServer, bool UseTls)? UserEmailInfo = EmailUserTrack(Email,Email.IndexOf('@') + 1) ?? throw new Exception();
            emailMessage.From.Add(new MailboxAddress(signature, "harchuchartem@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(Name, Email));
            emailMessage.Subject = (Subject != null) ? Subject : string.Empty;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Message
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(UserEmailInfo.Value.smtServer, UserEmailInfo.Value.port, UserEmailInfo.Value.UseTls);
               // await client.AuthenticateAsync("", "???");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        private (int port, string smtServer, bool UseTls)? EmailUserTrack(string userEm, int StartIndex)
        {
            (int, string, bool)? values = null;
            switch (userEm.Substring(StartIndex))
            {
                case "gmail.com":values = valuesToEmail[0]; break;
                case "mail.com": values = valuesToEmail[1]; break;
            }
            return values;
        }
    }
}
