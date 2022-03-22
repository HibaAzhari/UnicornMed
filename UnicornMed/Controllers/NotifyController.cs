using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnicornMed.BotLibrary.Helpers;
using UnicornMed.Common.Context;

namespace UnicornMed.Api.Controllers
{
    public class NotifyController
    {
        protected readonly INotificationHelper notificationHelper;
        public NotifyController(INotificationHelper notificationHelper)
        {
            this.notificationHelper = notificationHelper;
        }

        [AllowAnonymous]
        [HttpPost("sendMessage/{userId}")]
        public async Task ProactiveMessage(string userId, [FromQuery] string message)
        {
            await notificationHelper.SendMessage(userId, message);
        }
    }
}
