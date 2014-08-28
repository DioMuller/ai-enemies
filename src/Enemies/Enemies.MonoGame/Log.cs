using System;

namespace Enemies
{
    static class Log
    {
        public static void Debug(string tag, string message)
        {
            Console.WriteLine("Debug: {0} - {1}", tag, message);
        }

        public static void Error(string tag, Exception ex)
        {
            var oldColor = Console.ForegroundColor;
            Console.Write("Error: [{0}] - ", tag);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = oldColor;
            //Console.WriteLine(ex.StackTrace);
        }
    }
}
