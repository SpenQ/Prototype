﻿using Mpb.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Mpb.DAL.Converters
{
    /// <summary>
    /// This JsonConverted was created to prevent JSON-specific annotations or a parameterless constructor for the StateTransaction class.
    /// </summary>
    public class StateTransactionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(StateTransaction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string hash = (string)jo["Hash"];
            string signature = (string)jo["Signature"];

            var result = JsonConvert.DeserializeObject<StateTransaction>(jo.ToString());

            // Manually finalize the deserialized transaction to make it valid
            result.Finalize(hash, signature);

            return result;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}