using System;
using System.Text;

namespace PrettyConsoleOutput
{
    public static class PrettyConsole
    {
        public static void Write(string text, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Write(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
            var sb = new StringBuilder();
            sb.Append(value, times);
            Write(sb.ToString(), color);
        }

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.Yellow)
        {
            Write(text, color);
            Console.WriteLine();
        }

        public static void WriteLine(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
            var sb = new StringBuilder();
            sb.Append(value, times);
            Write(sb.ToString(), color);
            Console.WriteLine();
        }

        /// <summary>
        /// Logs an error to the console: [time]: message: error
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        public static void LogError(string message, Exception ex = null)
        {
            var sb = new StringBuilder($"[{DateTime.Now.ToShortTimeString()}]: ");
            sb.AppendLine(message);

            if (ex != null)
            {
                sb.AppendLine(ex.ToString());
            }
           
            WriteLine(sb.ToString(), ConsoleColor.Red);
        }
    }
}