using System.Collections.Generic;
using Newtonsoft.Json;

namespace SupercellProxy.JSON_Parser
{
    internal class ParsedPacket
    {
        /// <summary>
        ///     The Packet ID
        /// </summary>
        public int PacketID { get; set; }

        /// <summary>
        ///     The Payload Length
        /// </summary>
        public int PayloadLength { get; set; }

        /// <summary>
        ///     The Packet Name
        /// </summary>
        public string PacketName { get; set; }

        /// <summary>
        ///     The parsed Fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<object>> ParsedFields { get; set; }
    }
}