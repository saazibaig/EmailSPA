using EmailSPA.BusinessModel;
using System;
using System.Net.Mail;
using System.Web.Http;
using EmailSPA.BusinessServices;

namespace EmailSPA.API.Controllers
{
    public class EmailController : ApiController
    {

        /// <summary>
        /// send email to one or many receipients
        /// </summary>
        /// <param name="email"></param>
        /// <returns>returns response message</returns>
        [HttpPost]
        [ActionName("sendmail")]
        public IHttpActionResult SendEmail(EmailModel email)
        {
            try
            {
                var emailService = new EmailService();
                var response = emailService.SendEmail(email);
                return Ok<ActionResultResponse>(response);

            }
            catch (Exception ex)
            {
                return BadRequest("Exception occurred while sending email: " + ex);
            }

        }
    }
}
