using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercellProxy.JSON_Parser
{
    class ParsedPacket
    {
        /// <summary>
        /// The Packet ID
        /// </summary>
        public int PacketID
        {
            get;
            set;
        }

        /// <summary>
        /// The Payload Length
        /// </summary>
        public int PayloadLength
        {
            get;
            set;
        }

        /// <summary>
        /// The Packet Name
        /// </summary>
        public string PacketName
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed Ints, Little Endian Ints and VInts
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<object>> ParsedFields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed Ints, Little Endian Ints and VInts
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<int>> ParsedIntFields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed Int64, VInt64 and LittleEndianInt64
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<long>> ParsedInt64Fields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed UInt64 Fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<ulong>> ParsedUInt64Fields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed UInt Fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<uint>> ParsedUIntFields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed Short Fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<short>> ParsedShortFields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed UShort Fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<ushort>> ParsedUShortFields
        {
            get;
            set;
        }

        /// <summary>
        /// The parsed Strings and SupercellStrings
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<string>> ParsedStringFields
        {
            get;
            set;
        }
    }
}
