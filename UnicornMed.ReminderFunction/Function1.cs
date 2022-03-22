using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace UnicornMed.ReminderFunction
{
    public class Function1
    {

        protected readonly HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */5 * * * *",RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string userId = "29:1iAKxOgBHr_8At1yuxnvgy9zeILOL4a_RFamsskUji12AahYijpQ1c1OlPLRqbmB5eOz-fmoOd_gFgpD1IVhrkw";
            string message = "Proactive Message";

            string url = "https://localhost:3979/sendMessage/"+userId+"?";
            string param = $"message={message}";

            HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(url + param, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
