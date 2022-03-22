using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using UnicornMed.Common.Helpers.API.ResponseItems;

namespace UnicornMed.BotLibrary.Helpers
{
    public class AdaptiveCardHelper
    {
        public static List<Attachment> GetDoctorListCardActivity(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static Attachment GetDoctorById(DoctorItem doctor)
        {
            AdaptiveCard card = new AdaptiveCard("1.2");
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Name: " + doctor.FirstName + " " + doctor.LastName
            });
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Contact: " + doctor.Email
            });
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "ID: " + doctor.Id.ToString()
            });
            
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
        }
        public static Attachment GetDoctorAvailability(AvailabilityItem availability)
        {
            AdaptiveCard card = new AdaptiveCard("1.2");
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Name: " + availability.Doctor.FirstName + " " + availability.Doctor.LastName + "\n Contact: " + availability.Doctor.Email + "\n ID: " + availability.Doctor.Id
            });
            availability.Slots.ForEach(slot =>
            {
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Slot start: " + slot.Start,
                    Spacing = AdaptiveSpacing.None
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Slot end: " + slot.End,
                    Spacing = AdaptiveSpacing.None
                });
            });

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
        }
        public static List<Attachment> GetDoctorSchedule(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> NewBooking(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> CancelBooking(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> GetAllDoctorsAvailability(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> GetAllDoctorsSchedule(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> GetBooking(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> GetPatientHistory(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> GetMostBooked(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
        public static List<Attachment> GetBusyDoctors(List<DoctorItem> doctors)
        {
            List<Attachment> attachments = new List<Attachment>();
            doctors.ForEach(doctor =>
            {
                AdaptiveCard card = new AdaptiveCard("1.2");
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Name: " + doctor.FirstName + " " + doctor.LastName
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "Contact: " + doctor.Email
                });
                card.Body.Add(new AdaptiveTextBlock
                {
                    Text = "ID: " + doctor.Id.ToString()
                });
                attachments.Add(new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                });
            });

            return attachments;
        }
    }
}
