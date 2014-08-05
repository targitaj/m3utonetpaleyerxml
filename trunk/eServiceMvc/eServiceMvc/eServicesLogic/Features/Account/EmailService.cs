namespace Uma.Eservices.Logic.Features.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Implements MS Identity Fraework interface to supply means of sending e-mail for it.
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// This method should send the message
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public Task SendAsync(IdentityMessage message)
        {
            // TODO: Plug in your email service here to send an email.
            return Task.FromResult(0);
            
            // Example of sending mail through SMTP:
            /*
            var credentialUserName = "yourAccount@outlook.com";
            var sentFrom = "yourAccount@outlook.com";
            var pwd = "yourApssword";

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination) { Subject = message.Subject, Body = message.Body };

            // Send:
            return client.SendMailAsync(mail);
            
            */
        }
    }
}
