using AdaptiveCards;
using UnicornMed.Common.Context;
using UnicornMed.Common.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using UnicornMed.Common.Models.Database.API;
using Newtonsoft.Json;

namespace UnicornMed.BotLibrary.Helpers
{
    public class EmailPromptHelper : IEmailPromptHelper
    {
        protected readonly AppDbContext context;
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IConfiguration _configuration;
        protected readonly HttpClient client = new HttpClient();

        private string _activityId;

        public EmailPromptHelper(AppDbContext context, IBotFrameworkHttpAdapter adapter, IConfiguration _configuration)
        {
            this.context = context;
            this._adapter = adapter;
            this._configuration = _configuration;
        }

        public string JwtHandler(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            string userEmail = jwtToken.Claims.First(claim => claim.Type == "unique_name").Value;
            return userEmail;
        }

        public async Task SendPrompt(string userEmail)
        {
            var conRef = context.ConversationReferenceEntities.Where(r => r.RowKey == userEmail).FirstOrDefault();

            string botId = _configuration["MicrosoftAppId"];

            if (conRef != null)
            {
                ConversationReference reference = new ConversationReference()
                {
                    Conversation = new ConversationAccount()
                    {
                        Id = conRef.ConversationId
                    },
                    ServiceUrl = conRef.ServiceUrl,
                };

                await ((BotAdapter)_adapter).ContinueConversationAsync(
                       botId,
                       reference,
                       async (context, token) =>
                       {
                           IMessageActivity messageActivity = MessageFactory.Attachment(GetPrompt(conRef.Name));
                           await BotCallback(messageActivity, context, token);

                       },
                       default);
            }
        }

        public async Task SendBookingPrompt(string userEmail)
        {
            var conRef = context.ConversationReferenceEntities.Where(r => r.RowKey == userEmail).FirstOrDefault();

            string botId = _configuration["MicrosoftAppId"];

            if (conRef != null)
            {
                ConversationReference reference = new ConversationReference()
                {
                    Conversation = new ConversationAccount()
                    {
                        Id = conRef.ConversationId
                    },
                    ServiceUrl = conRef.ServiceUrl,
                };

                await ((BotAdapter)_adapter).ContinueConversationAsync(
                       botId,
                       reference,
                       async (context, token) =>
                       {
                           IMessageActivity messageActivity = MessageFactory.Attachment(GetBookingPrompt(conRef.Name));
                           await BotCallback(messageActivity, context, token);

                       },
                       default);
            }
        }

        public async Task RegisterEmail(dynamic email, dynamic name, dynamic department)
        {
            UserEntity user = new UserEntity
            {
                AltEmail = email,
                Name = name,
                Department = department

            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            SendMailHelper.SendMail(user);
        }

        public async Task MakeBooking(dynamic doctor, dynamic patient, dynamic start, dynamic end)
        {
            string url = "https://b01f-217-165-115-180.ngrok.io/api/Booking/new";
            Booking bookingObject = new Booking
            {
                Patient_Id = doctor,
                Doctor_Id = patient,
                StartTime = DateTime.Parse(start),
                EndTime = DateTime.Parse(end)
            };
            try
            {
               var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(bookingObject), Encoding.UTF8,"application/json"));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        public List<UserEntity> GetAllUsers()
        {
            return context.Users.ToList();
        }

        private async Task BotCallback(IMessageActivity message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(message);
        }

        public static Attachment GetPrompt(string name)
        {
            AdaptiveCard card = new AdaptiveCard("1.2");
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Hi " + name + "!",
                Weight = AdaptiveTextWeight.Bolder,
                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                Wrap = true
            });
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Please enter your Department and Alternate email address below:",
                Wrap = true
            });
            card.Body.Add(new AdaptiveTextInput
            {
                Placeholder = "Department",
                Id = "dept"
            });
            card.Body.Add(new AdaptiveTextInput
            {
                Placeholder = "Alternate Email Address",
                Id = "email"
            });
            AdaptiveTextInput email = (AdaptiveTextInput)card.Body.Where(el => el.Id == "email").FirstOrDefault();
            AdaptiveTextInput dept = (AdaptiveTextInput)card.Body.Where(el => el.Id == "dept").FirstOrDefault();
            // Do validations
            string data = "{ \"Name\": \""+name+"\", \"AltEmail\": \""+email.Value+"\", \"Department\": \""+ dept.Value + "\" }";
            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Submit",
                Id = "submit",
                DataJson = "{\"type\" : \"altEmail\"}"
            });
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

        }

        public static Attachment GetBookingPrompt(string name)
        {
            AdaptiveCard card = new AdaptiveCard("1.2");
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Hi " + name + "!",
                Weight = AdaptiveTextWeight.Bolder,
                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                Wrap = true
            });
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Please enter the details for your booking below:",
                Wrap = true
            });
            card.Body.Add(new AdaptiveNumberInput
            {
                Placeholder = "Doctor Id",
                Id = "Doctor"
            });
            card.Body.Add(new AdaptiveNumberInput
            {
                Placeholder = "Patient Id",
                Id = "Patient"
            });
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Booking Date:",
                Wrap = true
            });
            card.Body.Add(new AdaptiveDateInput{ Id = "date"});
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Starting at:",
                Wrap = true
            });
            card.Body.Add(new AdaptiveTimeInput { Id = "start" });
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Until:",
                Wrap = true
            });
            card.Body.Add(new AdaptiveTimeInput { Id = "end" });

            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = "Submit",
                Id = "submitBooking",
                DataJson = "{\"type\" : \"booking\"}"
            });
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

        }

        public UserEntity GetUserEntity(string email, string department)
        {
            return context.Users.Where(u => u.AltEmail==email && u.Department==department).FirstOrDefault();
        }
    }
}
