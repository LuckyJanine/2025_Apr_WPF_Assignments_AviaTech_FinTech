using FlowLedger.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlowLedger.Utils
{
    /// <summary>
    /// this util can be used for (de)serialization when Key of a dictionary is not a primitive type or a string
    /// or any object that is chose not to be stringified
    /// </summary>
    internal class DictionaryDTOConverter<TKey, TValue> : Newtonsoft.Json.JsonConverter
    {
        private class KeyValuePairWrapper
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Dictionary<TKey, TValue>).IsAssignableFrom(typeToConvert);
        }

        public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var dict = (Dictionary<TKey, TValue>)value;
            var items = dict.Select(kvp => new KeyValuePairWrapper 
            {
                Key = kvp.Key,
                Value = kvp.Value
            }).ToList();
            serializer.Serialize(writer, items);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var items = serializer.Deserialize<List<KeyValuePairWrapper>>(reader);

            if (items != null && items.Any())
            {
                return items.ToDictionary(item => item.Key, item => item.Value);
            }

            return new Dictionary<TKey, TValue>();
        }
    }
}
