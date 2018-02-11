using System.Web.Mvc;
using EmailSPA.BusinessModel;
using System.Net.Http;
using System;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmailSPA.UI.Controllers
{

    public class HomeController : Controller
    {
        private readonly string _emailApi;
        public HomeController()
        {
            _emailApi = ConfigurationManager.AppSettings["EMAILSPA_API"];
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Email(EmailModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                if (_emailApi.IsNullOrWhiteSpace())
                {
                    model.StatusMessage = "Email api is not defined in web config.";
                    return View(model);
                }

                //Email to
                model.Recipients = new List<EmailRecipientModel>();
                var emailList = model.RecipientList.Split(',');
                foreach (var email in emailList)
                {
                    model.Recipients.Add(new EmailRecipientModel { EmailAddress = email.Trim() });
                }

                //CC to 
                if (!model.CcRecipientList.IsNullOrWhiteSpace())
                {
                    model.CcRecipients = new List<EmailRecipientModel>();
                    var ccEmailList = model.CcRecipientList.Split(',');
                    foreach (var email in ccEmailList)
                    {
                        model.CcRecipients.Add(new EmailRecipientModel { EmailAddress = email.Trim() });
                    }
                }

                //Bcc to
                if (!model.BccRecipientList.IsNullOrWhiteSpace())
                {
                    model.BccRecipients = new List<EmailRecipientModel>();
                    var bccEmailList = model.BccRecipientList.Split(',');
                    foreach (var email in bccEmailList)
                    {
                        model.BccRecipients.Add(new EmailRecipientModel { EmailAddress = email.Trim() });
                    }
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_emailApi + "email/sendmail");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync("email", model);
                    postTask.Wait();

                    var jsonResult = await postTask.Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ActionResultResponse>(jsonResult);

                    model.StatusMessage = result.Status ? "Email sent succesfully" : result.Message;
                }

                return View(model);
            }
            catch (Exception e)
            {
                model.StatusMessage = e.Message;
                return View(model);
            }
        }


    }
}
