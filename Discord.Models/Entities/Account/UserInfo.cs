using EynwaDiscordBot.Models.Entities.Converter;
using EynwaDiscordBot.Models.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EynwaDiscordBot.Models.Entities.Account
{
    public class UserInfo
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("discordId")]
        public long DiscordId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
        [JsonConverter(typeof(CustomStringEnumConverter), true)]
        [JsonProperty("roles")]
        public Roles Roles { get; set; }
    }
}
