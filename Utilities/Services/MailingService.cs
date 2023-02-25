using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Bookstore.Utilities.Services {
    public class MailingService : IMailingService {
        private readonly MailSettings _mailingsetting;

        public MailingService(IOptions<MailSettings> mailingsetting)
        {
            _mailingsetting = mailingsetting.Value;
        }
        public async Task SendEmailAsync(string mailto, string subject, string body)
        {
            var Msg = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_mailingsetting.Email),
                Subject = subject ?? string.Empty
            };
            Msg.To.Add(MailboxAddress.Parse(mailto));
            var builder = new BodyBuilder();
            builder.TextBody = body;
            Msg.Body = builder.ToMessageBody();
            Msg.From.Add(new MailboxAddress(_mailingsetting.DisplayName, _mailingsetting.Email));
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailingsetting.Host, _mailingsetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailingsetting.Email, _mailingsetting.Password);
            await smtp.SendAsync(Msg);
            await smtp.DisconnectAsync(true);
        }
    }
}


