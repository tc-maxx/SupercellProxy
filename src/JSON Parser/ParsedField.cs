using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SupercellProxy.JSON_Parser
{
    class ParsedField<T>
    {
        public string FieldName { get; set; }
        public int FieldLength { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }
        public T FieldValue { get; set; }
    }
}
