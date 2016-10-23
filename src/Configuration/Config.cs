using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SupercellProxy
{
    class Config
    {
        private static Configuration Configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets a configuration value as string
        /// </summary>
        public static string Get(string key)
        {
            return Configuration.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// Sets a configuration value as string
        /// </summary>
        public static void Set(string key, string value)
        {
            Configuration.AppSettings.Settings[key].Value = value;
            Configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }


        public static bool JSON_Logging
        {
            get
            {
                bool ret = false;
                Boolean.TryParse(Get("JSON_Logging"), out ret);
                return ret;
            }
        }
        /// <summary>
        /// Returns the host
        /// </summary>
        public static string Host
        {
            get
            {
                return Get("Host").ToLower();
            }
        }

        /// <summary>
        /// Returns the game
        /// </summary>
        public static Game Game
        {
            get
            {
                if (Host == "game.clashofclans.com" || Host == "gamea.clashofclans.com")
                {
                    // Only configure the proxy to CoC if the host is valid
                    return Game.CLASH_OF_CLANS;
                }
                else if (Host == "game.clashroyaleapp.com")
                {
                    // Only configure the proxy to CR if the host is valid
                    return Game.CLASH_ROYALE;
                }
                else if(Host == "game.boombeachgame.com")
                {
                    return Game.BOOM_BEACH;
                }
                else
                {
                    Console.Clear();
                    Logger.Log("You configured an invalid host (" + Host + ")!", LogType.WARNING);
                    Logger.Log("Please check your config file!", LogType.WARNING);
                    Program.WaitAndClose();
                    return Game.CLASH_ROYALE;
                 }                    
            }
        }
    }
}
