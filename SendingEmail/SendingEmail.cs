
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Net.Http;

namespace SendingEmail
{
    public static class SendingEmail
    {
        [FunctionName("SendingEmail")]
        public static async Task<object> Run([HttpTrigger(AuthorizationLevel.Function)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Starting the mail sending process...");

            //Retrieval...
            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            //Name and mail
            string name = data.name;
            string mail = data.mail;

            //Questions
            string question1 = data.question1;
            string question2 = data.question2;
            string question3 = data.question3;
            string question4 = data.question4;
            string question5 = data.question5;
            string question6 = data.question6;
            string question7 = data.question7;
            string question8 = data.question8;
            string question9 = data.question9;
            string question10 = data.question10;

            //Email
            bool isImportantEmail = bool.Parse(data.isImportant.ToString());
            string fromEmail = data.fromEmail;
            string toEmail = data.toEmail;
            int smtpPort = 587;
            bool smtpEnableSsl = true;
            string smtpHost = "{smtp endpoint here}"; // your smtp host
            string smtpUser = "{smtp user here}"; // your smtp user
            string smtpPass = "{smtp password here}"; // your smtp password
            string subject = data.subject;
            string message = data.message;

            //Validation
            if (!ValidationPassed(question1, question2, question3, question4, question5, question6, question7, question8, question9, question10))
            {
                return SendMail(req, log, ref mail, isImportantEmail, fromEmail, toEmail, smtpPort, smtpEnableSsl, smtpHost, smtpUser, smtpPass, subject, message);
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.OK, new { status = true, message = string.Empty });
            }

            //Sending email
            
        }

        private static object SendMail(HttpRequestMessage req, ILogger log, ref string _mail, bool isImportantEmail, string fromEmail, string toEmail, int smtpPort, bool smtpEnableSsl, string smtpHost, string smtpUser, string smtpPass, string subject, string message)
        {
            MailMessage mail = new MailMessage(fromEmail, toEmail);
            SmtpClient client = new SmtpClient();
            client.Port = smtpPort;
            client.EnableSsl = smtpEnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = smtpHost;
            client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
            mail.Subject = subject;
            mail.IsBodyHtml = true;

            if (isImportantEmail)
            {
                mail.Priority = MailPriority.High;
            }

            mail.Body = message;
            try
            {
                client.Send(mail);
                log.LogInformation("Mail sent.");
                return req.CreateResponse(HttpStatusCode.OK, new { status = true, message = string.Empty });
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());
                return req.CreateResponse(HttpStatusCode.InternalServerError, new { status = false, message = "Message has not been sent. Check Azure Function Logs for more information." });
            }
        }

        public static bool ValidationPassed(string question1, string question2, string question3, string question4, string question5, string question6, string question7, string question8, string question9, string question10)
        {
            // Static implementation below. Ideally it should use Sentiment Analisys to automatically identify different # of responses.
            // Sentiment Analysis implementations is commented below.
            if ((question1 == "Dissatisfied" || question1 == "Very dissatisfied") ||
                   (question2 == "No" || question2 == "Not sure") ||
                   (question3 == "1" || question3 == "2" || question3 == "3" || question3 == "4") ||
                   (question4 == "Not so well" || question4 == "Not at all well") ||
                   (question5 == "Not so well" || question5 == "Not at all well") ||
                   (question6 == "Not so prepared" || question6 == "Not at all prepared") ||
                   (question7 == "Not so effective" || question7 == "Not at all effective") ||
                   (question8 == "Not so effective" || question8 == "Not at all effective") ||
                   (question9 == "No" || question9 == "Not sure") ||
                   (question10 == "1" || question10 == "2" || question10 == "3" || question10 == "4"))
            {
                return false;
            }
            else
            {
                return true;
            }

            /*
             *  // Calling Sentiment Analysis API to automatically verify the answers sentiment.
             * 
             *  // Considering answersList as "List<string> answersList = new List<string>();"
                foreach(string item in answersList)
                {
                    using (var client = new HttpClient())
                {
                    // Set up API call
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{your key here}");

                    var request = 
                        new { 
                            documents = new [] { 
                                new { 
                                    language = "en", 
                                    id = "001", 
                                    item 
                                    } 
                                } 
                            };

                    // Create form data, setting the content type
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    // Send it to Sentiment endpoint
                    var sentimentEndpoint = "https://westus2.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
                    var response = await client.PostAsync(sentimentEndpoint, content);

                    // Deserialize json response to dynamic object
                    dynamic dynamicResponse = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                    // Get the score as a number
                    float score = dynamicResponse.documents[0].score;

                    if(score > 0.5)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                }
             */
        }

        /*
         * Payload for test purposes
         * 
         * {
         *   "name":"",
         *   "mail":"",
         *   "question1":"",
         *   "question2": "",
         *   "question3":"",
         *   "question4":"",
         *   "question5":"",
         *   "question6":"",
         *   "question7":"",
         *   "question8":"",
         *   "question9":"",
         *   "question10":"",
         *   "fromEmail": "",
         *   "toEmail": "",
         *   "subject": "",
         *   "message": "",
         *   "isImportant": true
         * }
         * 
         * 
         * Payload
         * {
            "fromEmail": "no-reply@americasuniversity.com",
            "toEmail": "fabsanc@microsoft.com",
            "subject": "May we have 3 minutes of your time?",
            "message": "Message test",
            "isImportant": true
            }
         */
    }
}
