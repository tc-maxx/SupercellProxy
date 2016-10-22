using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SupercellProxy.JSON_Parser
{
    internal enum FieldType
    {
        String,
        SupercellString,
        LittleEndianInt,
        LittleEndianInt64,
        LittleEndianUInt,
        Int,
        Int64,
        LittleEndianUInt64,
        UInt,
        UInt64,
        VInt,
        VInt64,
        LittleEndianShort,
        LittleEndianUShort,
        Short,
        UShort
    }

    internal class JSONPacketField
    {
        /// <summary>
        ///     The Name of the Field
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     The Data Type of the Field
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }
    }
}