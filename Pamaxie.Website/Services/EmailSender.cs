using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Sql.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Pamaxie.Website.Services
{
    /// <summary>
    /// Service for everything related to sending emails
    /// </summary>
    public class EmailSender
    {
        private readonly string _email;
        private readonly string _userName;
        private readonly string _password;

        private readonly NavigationManager _navigationManager;
        
        public EmailSender(IConfiguration configuration, NavigationManager navigationManager)
        {
            IConfigurationSection emailConfigSection = configuration.GetSection("Email");
            _email = emailConfigSection.GetValue<string>("EmailAddress");
            _userName = emailConfigSection.GetValue<string>("UserName");
            _password = emailConfigSection.GetValue<string>("Password");
            _navigationManager = navigationManager;
        }
        
        /// <summary>
        /// Sends a confirmation email to the registered user.
        /// </summary>
        /// <param name="profile"></param>
        public async void SendConfirmationEmail(ProfileData profile)
        { 
            //TODO have a way to generate a email confirmation token.
            //string code = await _userManager.GenerateEmailConfirmationTokenAsync(profile).ConfigureAwait(false);
            string code = string.Empty;
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = _navigationManager.ToAbsoluteUri($"ConfirmEmail/{profile.EmailAddress}/{code}/").ToString();
            await SendEmailAsync(profile.EmailAddress, "Confirm Your Email", EmailBody.EmailConfirmationBody(callbackUrl)).ConfigureAwait(false);
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
                Host = _email,
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_userName, _password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                TargetName = "STARTTLS/outlook.office.com"
            };

            return smtpClient.SendMailAsync(mail);
        }
    }
#pragma warning restore

    /// <summary>
    /// Email Body Templates.
    /// </summary>
    public static class EmailBody
    {
        public static string EmailConfirmationBody(string callbackUrl)
        {
            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en-GB\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><title>Verify your Email</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/><style type=\"text/css\">a[x-apple-data-detectors] {color: inherit !important;}</style></head><body style=\"margin: 0; padding: 0; background: #313131\"><table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td style=\"padding: 20px 0 30px 0;\"><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border-collapse: collapse; border: 1px solid #212121;\"><tr><td align=\"center\" bgcolor=\"#70bbd9\"><img src=\"https://i.imgur.com/hMzkJiv.png\" alt=\"Creating Email Magic.\" width=\"600\" height=\"330\" style=\"display: block;\" /></td></tr><tr><td bgcolor=\"#212121\" style=\"padding: 40px 30px 40px 30px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;\"><tr><td style=\"color: #ffff; font-family: Arial, sans-serif;\"><h1 style=\"font-size: 24px; margin: 0;\">Please verify your email to start</h1></td></tr><tr><td style=\"color: #ffff; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 20px 0 30px 0;\"><p style=\"margin: 0;\">To start using your account you need to verfy your email here: " +
                $"{HtmlEncoder.Default.Encode(callbackUrl)}" +
                "</p></td></tr><tr><td><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;\"><tr><td width=\"260\" valign=\"top\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;\"><tr><td><img src=\"https://assets.codepen.io/210284/left_1.gif\" alt=\"\" width=\"100%\" height=\"140\" style=\"display: block;\" /></td></tr><tr><td style=\"color: #ffff; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 25px 0 0 0;\"><p style=\"margin: 0;\">If you require support you can contact us via this address and gain help: <button style=\" border: 0;background: #faac31;box-shadow: none;border-radius: 0px; padding: 4px;\"href=\"https://deamonic.freshdesk.com\">Contact Support</button></p></td></tr></table></td><td style=\"font-size: 0; line-height: 0;\" width=\"20\">&nbsp;</td><td width=\"260\" valign=\"top\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;\"><tr><td><img src=\"https://assets.codepen.io/210284/right_1.gif\" alt=\"\" width=\"100%\" height=\"140\" style=\"display: block;\" /></td></tr><tr><td style=\"color: #ffff; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 25px 0 0 0;\"><p style=\"margin: 0;\">We will also use this email to notify you if something suspcious happens on your account or if we suspect a databreach in any of our applications. We will never sent spam or advertising out without explicit agreement to do so.</p></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td bgcolor=\"#faac31\" style=\"padding: 30px 30px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;\"><tr><td style=\"color: #313131; font-family: Arial, sans-serif; font-size: 14px;\"><p style=\"margin: 0;\">&copy; 2020 Lukas Duerr e.K.<br/> </td><td align=\"right\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse;\"><tr><td><a href=\"http://www.twitter.com/leechfilcher\"><img src=\"https://i.imgur.com/wso2v2l.png\" alt=\"Twitter.\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" /></a></td><td style=\"font-size: 0; line-height: 0;\" width=\"20\">&nbsp;</td></tr></table></td></tr></table></td></tr></table></td></tr></table></body></html>";
        }

        public static string ResetPasswordBody(string callbackUrl)
        {
            //TODO Add a reset password email layout
            return $"{HtmlEncoder.Default.Encode(callbackUrl)}";
        }
    }
}