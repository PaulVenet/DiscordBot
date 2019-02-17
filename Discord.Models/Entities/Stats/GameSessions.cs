using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eynwa.Models.Entities.Stats
{
    public class GameSessions
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("gameName")]
        public string GameName { get; set; }
        [JsonProperty("timing")]
        public string Timing { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
