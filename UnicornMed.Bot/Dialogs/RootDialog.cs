using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json.Linq;
using Microsoft.Recognizers.Text;
using System.Collections.Generic;
using System;

namespace UnicornMed.Bot.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<JObject> _userStateAccessor;

        public RootDialog(UserState userState)
            : base("root")
        {
            _userStateAccessor = userState.CreateProperty<JObject>("result");

            var booking_details = new List<BookingDetails>
            {
                new BookingDetails("PatientId", "text", "Enter patient ID to book for"),
                new BookingDetails("DoctorId", "text", "Enter doctor ID to book with"),
                //new BookingDetails("Date", "date", "When would you like your appointment?"),
                new BookingDetails("Duration", "number", "How long would you like to book for?")
            };

            //AddDialog(new TextPrompt("text"));
            //AddDialog(new DateTimePrompt("date", new PromptValidator<DateTimeResolution>(PromptValidatorContext<T>) => Promise<boolean>));

            //var date = new List<BookingDetails>
            //{
            //    new BookingDetails("day","")
            //};
        }
    }
}
