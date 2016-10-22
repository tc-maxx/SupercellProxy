using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SupercellProxy.JSON_Parser
{
    internal class ParsedField<T>
    {
        /// <summary>
        /// Name of the Field
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Length of the Field in Bytes
        /// </summary>
        public int FieldLength { get; set; }

        /// <summary>
        /// Datatype of the Field
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        /// <summary>
        /// The Parsed Value 
        /// </summary>
        public T FieldValue { get; set; }
    }
}