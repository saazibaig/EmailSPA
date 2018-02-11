using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmailSPA.BusinessModel
{
    public class EmailModel
    {
        [Required, Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required, Display(Name = "Message")]
        public string Body { get; set; }

        [Required, Display(Name = "Email To")]
        public string RecipientList { get; set; }

        [Display(Name = "CC")]
        public string CcRecipientList { get; set; }

        [Display(Name = "BCC")]
        public string BccRecipientList { get; set; }

        public List<EmailRecipientModel> Recipients { get; set; }
        public List<EmailRecipientModel> CcRecipients { get; set; }
        public List<EmailRecipientModel> BccRecipients { get; set; }

        public string StatusMessage { get; set; }
    }

    public class EmailRecipientModel
    {
        public string EmailAddress { get; set; }
    }
}
