using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EynwaDiscordBot.Models.Entities.Converter
{
    public class CustomStringEnumConverter : StringEnumConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
        /// </summary>
        public CustomStringEnumConverter()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
        /// </summary>
        /// <param name="camelCaseText"><c>true</c> if the written enum text will be camel case; otherwise, <c>false</c>.</param>
        public CustomStringEnumConverter(bool camelCaseText)
            : base(camelCaseText)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while deserialization so default enum value has been set " + e.ToString());
                //return default enum value
                return Activator.CreateInstance(objectType);
            }
        }
    }
}
