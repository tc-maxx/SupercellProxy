using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SupercellProxy
{
    class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        static void Main(string[] args)
        {
            // Parse console args
            new ConsoleArgs(args).Parse();

            // Console title
            Console.Title = "▁ ▂ ▄ ▅ ▆ ▇ █     SupercellProxy - " + Helper.AssemblyVersion + "     █ ▇ ▆ ▅ ▄ ▂ ▁";

            // Disable resizing/maximizing
            NativeCalls.DisableMenus();

            // Start Proxy
            Proxy.Start();

            // Start command listener
            new ConsoleCmdListener();
        }

        /// <summary>
        /// Closes the program
        /// </summary>
        public static void Close()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Waits a certain amount of seconds and closes the program
        /// </summary>
        public static void WaitAndClose(int ms = 1350)
        {
            Thread.Sleep(ms);
            Close();
        }

        /// <summary>
        /// Restarts the program
        /// </summary>
        public static void Restart()
        {
            Process.Start(Assembly.GetExecutingAssembly().Location);
            Close();
        }
    }
}