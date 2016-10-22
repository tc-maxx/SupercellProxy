using System;
using System.IO;
using System.Text;
using SupercellProxy.JSON_Parser;

namespace SupercellProxy
{
    enum LogType
    {
        INFO, // A normal text (i.e. "Proxy started")
        WARNING, // A warning (i.e. 2 running proxys)
        CONFIG, // A configuration value (i.e. "Host")
        PACKET, // A client/server packet (i.e. KeepAlive)
        API, // An API message (i.e. "WebAPI started")
        FIELD,
        PACKETINFO,
        EXCEPTION // An exception (i.e. NullReferenceException)
    }

    

    static class Logger
    {
        static Logger()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
        }

        public static void CenterStr(string str)
        {
            // (window width - strlen) / 2 = center
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, Console.CursorTop);
            Console.WriteLine(str);
            // reset
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }
        
        private static void LogParsed(string prefix, string toLog, LogType l)
        {

            Console.ForegroundColor = l == LogType.FIELD ? ConsoleColor.Blue : l == LogType.PACKETINFO ? ConsoleColor.Magenta : ConsoleColor.Black;

            Console.Write("[{0}] ", prefix);

            Console.ResetColor();
           
            Console.WriteLine(toLog);
        }

        public static void LogParsedPacket(ParsedPacket pp)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            LogParsed(pp.PacketName, "PacketID: " + pp.PacketID, LogType.PACKETINFO);
            LogParsed(pp.PacketName, "PayloadLength: " + pp.PayloadLength, LogType.PACKETINFO);

            if (pp.ParsedFields != null)
            {
                foreach (var v in pp.ParsedFields)
                {
                    LogParsed(v.FieldName, "FieldLength: " + v.FieldLength, LogType.FIELD);
                    LogParsed(v.FieldName, "FieldType: " + v.FieldType, LogType.FIELD);
                    LogParsed(v.FieldName, "FieldValue: " + v.FieldValue, LogType.FIELD);
                }
            }
        }

        /// <summary>
        /// Logs passed text
        /// </summary>
        public static void Log(string text, LogType type = LogType.INFO)
        {
            // Print line out
            switch (type)
            {
                case LogType.INFO:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.API:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogType.PACKET:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case LogType.CONFIG:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }

            Console.Write("[" + type + "] ");
            Console.ResetColor();
            Console.WriteLine(text);

            // Log line to file
            string path = Environment.CurrentDirectory + @"\\Logs\\" + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy") + ".log";
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter StreamWriter = new StreamWriter(fs))
                {
                    StreamWriter.WriteLine("[" + DateTime.UtcNow.ToLocalTime().ToString("hh-mm-ss") + "-" + type + "] " + text);
                    StreamWriter.Close();
                }
            }
        }
    }
}
