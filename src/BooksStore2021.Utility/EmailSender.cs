using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BooksStore2021.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSettings _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();
            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages,
             new JArray
                 {
                    new JObject
                    {
                        {
                            "From",
                            new JObject
                            {
                                {"Email", "davidwu2021@protonmail.com"},
                                {"Name", "David"}
                            }
                        },
                        {
                            "To",
                            new JArray
                            {
                                new JObject
                                {
                                    {"Email", email},
                                }
                            }
                        },
                        {
                            "Subject",
                            subject
                        },
                        {
                            "HTMLPart",
                            body
                        }
                    }
                }
             );

            var response = await client.PostAsync(request);
        }
    }
}
