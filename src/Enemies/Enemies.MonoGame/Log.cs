using System;

namespace Enemies
{
    static class Log
    {
        public static void Debug(string message)
        {
            Console.WriteLine("Debug: " + message);
        }

        public static void Error(Exception ex)
        {
            var oldColor = Console.ForegroundColor;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = oldColor;
            //Console.WriteLine(ex.StackTrace);
        }
    }
}
