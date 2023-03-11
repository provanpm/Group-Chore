using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class Email : MonoBehaviour
{
    public void SendEmail(string recipientEmail, string subjectText, string bodyText)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;

        mail.From = new MailAddress("groupchore@gmail.com");
        mail.To.Add(new MailAddress(recipientEmail));

        mail.Subject = subjectText;
        mail.Body = bodyText;

        SmtpServer.Credentials = new System.Net.NetworkCredential(mail.From.ToString(), "smuysztgvmosjotb") as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        SmtpServer.Send(mail);
    }
}