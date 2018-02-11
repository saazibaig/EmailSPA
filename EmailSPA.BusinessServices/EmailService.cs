using System;
using EmailSPA.BusinessModel;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using RestSharp;
using RestSharp.Authenticators;
using System.Configuration;
using System.Collections.Generic;

namespace EmailSPA.BusinessServices
{
    public class EmailService
    {
        private readonly string _emailFrom;
        public EmailService()
        {
            _emailFrom = ConfigurationManager.AppSettings["EMAIL_FROM"];
        }

        public ActionResultResponse SendEmail(EmailModel email)
        {
            ActionResultResponse response = new ActionResultResponse();

            try
            {
                // Using Service Provider 1 for sending email
                var responseSendMailgun = SendMailgunProvider(email);
                if (responseSendMailgun.IsSuccessful)
                {
                    response.Status = true;
                    response.Message = "Success";
                    return response;
                }
                else
                {
                    response.Status = false;
                    response.Message = responseSendMailgun.Content;
                }


                // if Service Provider 1 failed then without effecting user use Service Provider 2
                var responseSendGrid = SendGridProvider(email);
                if (responseSendGrid.Status == TaskStatus.RanToCompletion)
                {
                    response.Status = true;
                    response.Message = "Success";
                    return response;
                }
                else
                {
                    response.Status = false;
                    if (responseSendGrid.Exception != null) response.Message = responseSendGrid.Exception.Message;
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public RestResponse SendMailgunProvider(EmailModel email)
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api",
                    ConfigurationManager.AppSettings["SENDMAILGUN_KEY"])
            };
            var request = new RestRequest();
            request.AddParameter("domain", "sandboxe0fd2839cdc84f608c92cc70c14972b7.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandboxe0fd2839cdc84f608c92cc70c14972b7.mailgun.org>");
            request.AddParameter("to", "Saad <saazi_baig@yahoo.com>"); // Note: not able to cover multiple reciepients, cc and bcc
            request.AddParameter("subject", email.Subject);
            request.AddParameter("text", email.Body);
            request.Method = Method.POST;
            return client.Execute(request) as RestResponse;

        }

        public async Task SendGridProvider(EmailModel email)
        {

            var apiKey = ConfigurationManager.AppSettings["SENDGRID_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_emailFrom);
            var subject = email.Subject;
            var tos = new List<EmailAddress>();

            foreach (var recipients in email.Recipients)
            {
                var to = new EmailAddress(recipients.EmailAddress);
                tos.Add(to);
            }

            var plainTextContent = email.Body;
            var htmlContent = "<strong>" + email.Body + "</strong>";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent); // Note: not able to cover cc and bcc
            await client.SendEmailAsync(msg);

        }

    }
}
