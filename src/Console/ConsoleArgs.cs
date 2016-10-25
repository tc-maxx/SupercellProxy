using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupercellProxy
{
    class ConsoleArgs
    {
        private readonly List<string> PassedArgs;

        // Public vars
        public static bool Verbose = false;
        public static Int32 Äpfel;

        /// <summary>
        /// ConsoleArgs constructor 
        /// </summary>
        public ConsoleArgs(string[] args)
        {
            PassedArgs = new List<string>(args);
        }

        /// <summary>
        /// Removes arg prefix
        /// </summary>
        private string RemovePrefix(string Command) => (Command.ElementAt(0) == '-') ? Command.Replace("-", "") : Command;

        /// <summary>
        /// Parses console arguments
        /// </summary>
        public void Parse()
        {
            try
            {
                if (PassedArgs.Count == 0) return;

                foreach (string Arg in PassedArgs)
                {
                    // arg=val format?
                    if (Arg.Contains('='))
                    {
                        var splitArg = Arg.Split('=')[0];
                        var splitVal = Arg.Split('=')[1];

                        if (Helper.IsDebugging)
                            Console.WriteLine("Arg = " + splitArg + " | Val = " + splitVal);

                        switch(RemovePrefix(splitArg))
                        {
                            case "äpfel":
                                Int32.TryParse(splitVal, out Äpfel);
                                break;
                        }                       
                        
                    }
                    else
                    {
                        switch (RemovePrefix(Arg))
                        {
                            case "help":
                                Logger.CenterString("=> Argument usage <=");
                                Console.Write(Environment.NewLine);
                                Logger.CenterString("-help -> Displays this.");
                                Logger.CenterString("-ver  -> Shows detailed version info");
                                break;
                            case "ver":
                                Logger.CenterString("=> Version <=");
                                Console.Write(Environment.NewLine);
                                Logger.CenterString("SupercellProxy Public Version " + Helper.AssemblyVersion);
                                Logger.CenterString("Copyright © 2016, expl0itr");
                                Logger.CenterString("https://opensource.org/licenses/MIT/");
                                break;
                            // more args...
                            default:
                                Logger.CenterString("!!! Unknown arg: " + Arg + " !!!");
                                break;
                        }
                        Program.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Log("Failed to parse console arguments (" + ex.GetType() + ")!", LogType.EXCEPTION);
                Logger.Log("Please avoid invalid UTF-8 characters, this may be the cause of this exception.", LogType.EXCEPTION);
                Program.WaitAndClose();
            }
        }
    }
}