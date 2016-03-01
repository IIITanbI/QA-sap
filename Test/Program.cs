using QA.AutomatedMagic;
using QA.AutomatedMagic.ApiManager;
using SapAutomation.AemPageManager;
using System;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = new Logger();

            Console.ReadLine();
        }
    }

    public class Logger : ILogger
    {
        public void DEBUG(string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }

        public void ERROR(string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }

        public void FATAL(string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }

        public void INFO(string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }

        public void LOG(string level, string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }

        public void TRACE(string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }

        public void WARN(string message, Exception exception = null)
        {
            Console.WriteLine(message);
        }
    }
}
