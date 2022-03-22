// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Text.Json;
using UnicornMed.Common.Helpers.API.ResponseItems;
using System.Linq;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using UnicornMed.BotLibrary.Helpers;

namespace UnicornMed.Bots.Bot
{
    public class Bot : TeamsActivityHandler
    //where T : Dialog
    {
        protected readonly IConversationReferencesHelper _conversationReferenceHelper; 
        protected readonly BotState ConversationState;
        protected readonly Dialog Dialog;
        protected readonly BotState UserState;
        protected readonly HttpClient client = new HttpClient();
        //private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;

        public Bot(IConversationReferencesHelper conversationReferencesHelper, ConversationState conversationState, UserState userState)
        {
            //, T dialog
            _conversationReferenceHelper = conversationReferencesHelper; 
            ConversationState = conversationState;
            UserState = userState;
            //Dialog = dialog;
        }

        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            await _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
        }
        protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

            if (activity.Action.Equals("add"))
                await _conversationReferenceHelper.AddorUpdateConversationRefrenceAsync(botConRef, currentMember);
            else if (activity.Action.Equals("remove"))
                await _conversationReferenceHelper.DeleteConversationRefrenceAsync(botConRef, currentMember);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to UnicornMed bot"), cancellationToken);
                }
            }
        }

        //private void AddConversationReference(Activity activity)
        //{
        //    var conversationReference = activity.GetConversationReference();
        //    _conversationReferences.AddOrUpdate(conversationReference.User.Id, conversationReference, (key, newValue) => conversationReference);
        //}

        //protected override Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    AddConversationReference(turnContext.Activity as Activity);

        //    return base.OnConversationUpdateActivityAsync(turnContext, cancellationToken);
        //}

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Text.ToLower().Contains("get all doctors"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get doctor"))
            {
                int id = 1; // get from user input text
                var response = await client.GetAsync("https://localhost:5001/api/doctor/"+id);
                DoctorItem doctor = JsonConvert.DeserializeObject<DoctorItem>(response.Content.ReadAsStringAsync().Result);
                if (doctor != null)
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorById(doctor));
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Doctor not found"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get free slots"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get schedule"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("New booking"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("cancel booking"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get all slots"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get all schedules"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get booking"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get history"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get most booked"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
            if (turnContext.Activity.Text.ToLower().Contains("get busy"))
            {
                var response = await client.GetAsync("https://localhost:5001/api/doctor/doctors");
                IEnumerable<DoctorItem> doctors = JsonConvert.DeserializeObject<List<DoctorItem>>(response.Content.ReadAsStringAsync().Result);
                if (doctors != null && doctors.Any())
                {
                    var res = MessageFactory.Attachment(AdaptiveCardHelper.GetDoctorListCardActivity(doctors.ToList()));
                    res.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await turnContext.SendActivityAsync(res, cancellationToken);
                }
                else
                    await turnContext.SendActivityAsync(MessageFactory.Text($"No doctors on record"), cancellationToken);
                return;
            }
        }
    }
}
