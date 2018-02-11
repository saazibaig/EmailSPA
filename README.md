# EmailSPA
This service accepts the necessary information and sends emails by using providers Mailgun and Sendgrid.

Architecture:

This solution 'EmailSPA' is a single page application which sends emails by calling rest api. It consists of four projects

1) EmailSPA.API
2) EmailSPA.BusinessModel
3) EmailSPA.BusinessServices
4) EmailSPA.UI


1)	EmailSPA.API 
		Is a web api project which has the logic of sending email by using libraries like EmailSPA.BusinessServices and EmailSPA.BusinessModel. The 'EmailController.cs' has the endpoint for calling api. We need to define the sendergrid and mailgun api keys in web.config of this website.
The api need to be deployed separately so to make it secure and independent from frontend web project. It can be accessed by following uri

http://localhost:62578/api/email/sendmail

 and following is the json payload

{
  "Subject": "sample string 1",
  "Body": "sample string 2",
  "RecipientList": "sample string 3",
  "CcRecipientList": "sample string 4",
  "BccRecipientList": "sample string 5",
  "Recipients": [
    {
      "EmailAddress": "sample string 1"
    },
    {
      "EmailAddress": "sample string 1"
    }
  ],
  "CcRecipients": [
    {
      "EmailAddress": "sample string 1"
    },
    {
      "EmailAddress": "sample string 1"
    }
  ],
  "BccRecipients": [
    {
      "EmailAddress": "sample string 1"
    },
    {
      "EmailAddress": "sample string 1"
    }
  ],
  "StatusMessage": "sample string 6"
}


2)	EmailSPA.BusinessModel
				Is a helping class library projects which contains the model of email objects. This project contains two classes ‘ActionResultResponse’ and ‘EmailModel’.


3)	EmailSPA.BusinessServices
Contains the core business logic for sending emails by using different providers. ‘EmailService’ is the class which is doing this job.

4)	EmailSPA.UI
Is the frontend website which contains the email form. This is a single page application. The form can be accessed by following uri

http://localhost:61002/home/email

The logic for sending email by calling EmailSPA.API endpoint is written in HomeController. This website needs to deploy alone as well. We need to define the url of api in web.config of this website.

Exceptions:

Although all the requirements from Single Page web app challenge has been meet except few which are following

1)	The email form is capable of sending email to multiple sender emails, multiple CC and multiple BCC but due to lack of understanding of third party email providers, I was not able to implement it in Sendgrid and Mailgun level.
2)	The user interface of email form is not clean enough or has some design flaws



Deployment:
		Publish EmailSPA.API and EmailSPA.UI individually and deploy on web server. The url of API need to be mentioned in UI web.config file. The email form in UI will be accessed through this url

http://xxxxxxxx/home/email
