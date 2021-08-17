using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Sql.Data;
using WebMarkupMin.Core;

namespace Pamaxie.Website.Services
{
    /// <summary>
    /// Service for everything related to sending emails
    /// </summary>
    public class EmailSender
    {
        private readonly string _email;
        private readonly string _password;

        private readonly NavigationManager _navigationManager;
        private readonly UserService _userService;

        public EmailSender(IConfiguration configuration, NavigationManager navigationManager, UserService userService)
        {
            IConfigurationSection emailConfigSection = configuration.GetSection("Email");
            _email = emailConfigSection.GetValue<string>("EmailAddress");
            _password = emailConfigSection.GetValue<string>("Password");
            _navigationManager = navigationManager;
            _userService = userService;
        }

        /// <summary>
        /// Sends a confirmation email to the registered user.
        /// </summary>
        /// <param name="profile"></param>
        public async void SendConfirmationEmail(ProfileData profile)
        {
            string code = _userService.GenerateEmailConfirmationToken(profile);
            string callbackUrl = _navigationManager.ToAbsoluteUri($"ConfirmEmail/{code}/").ToString();
            await SendEmailAsync(profile.EmailAddress, "Confirm Your Email",
                EmailBody.EmailConfirmationBody(callbackUrl)).ConfigureAwait(false);
        }

        private Task SendEmailAsync(string email, string subject, string body)
        {
            return Task.Run(() => Execute(email, subject, body));
        }

        private Task Execute(string email, string subject, string body)
        {
            MailMessage mail = new()
            {
                From = new MailAddress(_email, "Pamaxie"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(email);

            SmtpClient smtpClient = new()
            {
                Host = "smtp.privateemail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            return smtpClient.SendMailAsync(mail);
        }
    }

    /// <summary>
    /// Email Body Templates.
    /// </summary>
    public static class EmailBody
    {
        private static readonly string EmailConfirmationHtml = string.Empty;

        static EmailBody()
        {
            const string emailConfirmationPath = "EmailLayouts/EmailConfirmation.xhtml";
            if (File.Exists(emailConfirmationPath))
                EmailConfirmationHtml = MinifyHtmlPage(File.ReadAllText(emailConfirmationPath));
        }

        public static string EmailConfirmationBody(string callbackUrl)
        {
            return EmailConfirmationHtml.Replace("CALLBACKURL", HtmlEncoder.Default.Encode(callbackUrl));
        }

        private static string MinifyHtmlPage(string html)
        {
            HtmlMinifier htmlMinifier = new();
            MarkupMinificationResult result = htmlMinifier.Minify(html);
            return result.MinifiedContent;
        }
    }
}