using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BooksStore2021.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }
        static async Task RunAsync()
        {
            MailjetClient client = new MailjetClient("aebc03f5af0232f5e9bcdb7ddf6095fb", "85be983aa2b2e17ace7cc30ee0868e72");
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
                                {
                                    "Email",
                                    "david.dong.guo@gmail.com"
                                },
                                {
                                    "Name",
                                    "David"
                                }
                            }
                        }
                    },
                    {
                        "Subject",
                        "Greetings from Mailjet."
                    },
                    {
                        "TextPart",
                        "My first Mailjet email"
                    },
                    {
                            "HTMLPart",
                        "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
                    },
                    {
                        "CustomID",
                        "AppGettingStartedTest"
                    }
                }
             });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                System.Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                System.Console.WriteLine(response.GetData());
            }
            else
            {
                System.Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                System.Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                System.Console.WriteLine(response.GetData());
                System.Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }
    }
}
