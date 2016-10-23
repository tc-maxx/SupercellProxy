using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SupercellProxy
{
    internal class JSONPacketManager
    {
        public static Dictionary<int, JSONPacketWrapper> JsonPackets = new Dictionary<int, JSONPacketWrapper>();

        /// <summary>
        ///     Handle the Packet ( Parse Fields, Log Fields, Export as JSON )
        /// </summary>
        /// <param name="packet">The Instance of the Packet to Handle</param>
        public static void HandlePacket(Packet packet)
        {
            ParsedPacket pp;
            JSONPacketWrapper wrapper = null;

            if (JsonPackets.ContainsKey(packet.ID))
            {
                wrapper = JsonPackets[packet.ID];

                pp = JsonParseHelper.ParsePacket(wrapper, packet);
            }
            else
            {
                pp = new ParsedPacket();

                var psfields = new List<ParsedField<object>>();

                pp.PacketID = packet.ID;
                pp.PacketName = "Unknown";
                pp.PayloadLength = packet.DecryptedPayload.Length;

                psfields.Add(new ParsedField<object>
                {
                    FieldLength = packet.DecryptedPayload.Length,
                    FieldName = "Payload",
                    FieldType = FieldType.String,
                    FieldValue = packet.DecryptedPayload.ToHexString().Replace("-", "")
                });

                pp.ParsedFields = psfields;
            }

            if ((wrapper == null) || wrapper.ShouldLog || wrapper.ShouldExport)
            {
                if (((wrapper == null) || wrapper.ShouldLog) && Config.JSON_Logging)
                    Logger.LogParsedPacket(pp);

                if ((wrapper == null) || wrapper.ShouldExport)
                {
                    var path = @"Packets\\" + Config.Game + "_" + pp.PacketID + "_" +
                               string.Format("{0:dd-MM_hh-mm-ss}", DateTime.Now) + ".json";

                    using (var file = File.CreateText(path))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, pp);
                    }
                }
            }
        }

        /// <summary>
        ///     Loads all Packet Definitions into the List
        /// </summary>
        public static void LoadDefinitions()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory + @"\\JsonPackets\\", "*.json",
                SearchOption.AllDirectories);

            foreach (var filePath in files)
                using (var file = File.OpenText(filePath))
                {
                    var serializer = new JsonSerializer();

                    var wrapper = (JSONPacketWrapper) serializer.Deserialize(file, typeof(JSONPacketWrapper));

                    JsonPackets.Add(wrapper.PacketID, wrapper);
                }
        }

        /// <summary>
        ///     Initialize Packets
        /// </summary>
        public static void Initialize()
        {
            LoadDefinitions();
            Logger.Log(JsonPackets.Count + " JSON definitions LOADED!");
        }
    }
}