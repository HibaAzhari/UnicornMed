using UnicornMed.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnicornMed.BotLibrary.Helpers
{
    public interface IEmailPromptHelper
    {
        public Task SendPrompt(string aadObjectId);
        public Task SendBookingPrompt(string userEmail);
        public List<UserEntity> GetAllUsers();
        public string JwtHandler(string token);
        public Task RegisterEmail(dynamic email, dynamic name, dynamic department);
        public Task MakeBooking(dynamic doctor, dynamic patient, dynamic start, dynamic end);
        public UserEntity GetUserEntity(string email, string department);
    }
}
