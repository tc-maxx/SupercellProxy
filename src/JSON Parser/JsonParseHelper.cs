using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SupercellProxy.JSON_Parser
{
    internal class JsonParseHelper
    {
        /// <summary>
        ///     Calculates the lenght in bytes of an object
        ///     and returns the size
        /// </summary>
        /// <param name="TestObject"></param>
        /// <returns></returns>
        private static int GetObjectSize(object TestObject)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, TestObject);
            Array = ms.ToArray();
            return Array.Length;
        }

        public static ParsedPacket ParsePacket(JSONPacketWrapper wrapper, Packet p)
        {
            var pack = new ParsedPacket();

            pack.PacketID = p.ID;
            pack.PacketName = wrapper.PacketName;
            pack.PayloadLength = p.DecryptedPayload.Length;

            var parsedFields = new List<ParsedField<object>>();

            using (var br = new BinaryReader(new MemoryStream(p.DecryptedPayload)))
            {
                foreach (var field in wrapper.Fields)
                    try
                    {
                        var value = br.ReadField(field.FieldType);

                        parsedFields.Add(new ParsedField<object>
                        {
                            FieldLength = GetObjectSize(value),
                            FieldName = field.FieldName,
                            FieldType = field.FieldType,
                            FieldValue = value
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Exception occured while Parsing Packet: " + ex);
                    }
            }

            pack.ParsedFields = parsedFields.Count > 0 ? parsedFields : null;

            return pack;
        }
    }
}