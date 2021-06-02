using System;
using System.Text;

namespace PrettyConsoleHelper
{
    public static class PrettyConsole
    {
        public static ConsoleColor WriteColor = ConsoleColor.Yellow;
        public static ConsoleColor WriteLineColor = ConsoleColor.Cyan;

        /// <summary>
        /// Writes text to console using default console color
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string text)
        {
            Write(text, WriteColor);
        }

        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Write(object value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void Write(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
            var sb = new StringBuilder();
            sb.Append(value, times);
            Write(sb.ToString(), color);
        }

        /// <summary>
        /// Writes text to console using default console color
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLine(string text)
        {
            WriteLine(text, WriteLineColor);
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Write(text, color);
            Console.WriteLine();
        }

        public static void WriteLine(object value)
        {
            Write(value, WriteLineColor);
            Console.WriteLine();
        }

        public static void WriteLine(object value, ConsoleColor color)
        {
            Write(value, color);
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