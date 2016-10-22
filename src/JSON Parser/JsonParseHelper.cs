using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SupercellProxy.JSON_Parser
{
    class JsonParseHelper
    {
        /// <summary>
        /// Calculates the lenght in bytes of an object 
        /// and returns the size 
        /// </summary>
        /// <param name="TestObject"></param>
        /// <returns></returns>
        private static int GetObjectSize(object TestObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, TestObject);
            Array = ms.ToArray();
            return Array.Length;
        }

        public static ParsedPacket ParsePacket(JSONPacketWrapper wrapper, Packet p)
        {
            ParsedPacket pack = new ParsedPacket();

            pack.PacketID = p.ID;
            pack.PacketName = wrapper.PacketName;
            pack.PayloadLength = p.DecryptedPayload.Length;

            List<ParsedField<object>> parsedFields = new List<ParsedField<object>>();

            using (BinaryReader br = new BinaryReader(new MemoryStream(p.DecryptedPayload)))
            {
                foreach (JSONPacketField field in wrapper.Fields)
                {
                    object value = br.ReadField(field.FieldType);

                    parsedFields.Add(new ParsedField<object>()
                    {
                        FieldLength = GetObjectSize(value),
                        FieldName = field.FieldName,
                        FieldType = field.FieldType,
                        FieldValue = value
                    });

                }
            }

            pack.ParsedFields = parsedFields.Count > 0 ? parsedFields : null;

            return pack;
        }
    }
}
