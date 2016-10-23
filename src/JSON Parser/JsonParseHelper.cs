using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SupercellProxy
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
            if (TestObject != null)
            {
                using (Stream stream = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(stream, TestObject);
                    return (int) stream.Length;
                }
            }
            else
                return 0;
        }

        private static string ReplaceWildcards(string toSearch, List<ParsedField<object>> ParsedFields)
        {
            var result = ParsedFields.Find(x => x.FieldName == toSearch);

            if (result != null)
            {
                return Convert.ToString(result.FieldValue);
            }

            return null;
        }

        private static bool isNumber(string toCheck)
        {
            int temp;
            return int.TryParse(toCheck, out temp);
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
                {
                    try
                    {
                        if (field.FieldType == FieldType.Bytes)
                        {
                            int toRead = 0;
                            string replaced = ReplaceWildcards(field.BytesToRead, parsedFields);

                            if (replaced != null)
                            {
                                if (isNumber(replaced))
                                {
                                    toRead = Convert.ToInt32(replaced);
                                }
                            }
                            else
                            {
                                if (isNumber(field.BytesToRead))
                                {
                                    toRead = Convert.ToInt32(field.BytesToRead);
                                }
                            }

                            if (toRead > 0)
                            {
                                var value = br.ReadBytes(toRead);

                                parsedFields.Add(new ParsedField<object>()
                                {
                                    FieldLength = value.Length,
                                    FieldName = field.FieldName,
                                    FieldType = FieldType.Bytes,
                                    FieldValue = BitConverter.ToString(value)
                                });
                            }
                        }
                        else
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
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Exception occured while Parsing Packet: " + ex);
                    }

                }
                    
            }

            pack.ParsedFields = parsedFields.Count > 0 ? parsedFields : null;

            return pack;
        }
    }
}