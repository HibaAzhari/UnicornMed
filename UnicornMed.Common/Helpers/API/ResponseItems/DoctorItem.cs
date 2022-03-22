using Newtonsoft.Json;
using System.Collections.Generic;

namespace UnicornMed.Common.Helpers.API.ResponseItems
{
    public class DoctorItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }

    }
}
