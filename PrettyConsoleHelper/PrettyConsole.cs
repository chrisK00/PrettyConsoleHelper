using System;
using System.Text;
using Microsoft.Extensions.Options;

namespace PrettyConsoleHelper
{
    public class PrettyConsole : IPrettyConsole
    {
        public PrettyConsoleOptions Options { get; init; } = new();

        public PrettyConsole(PrettyConsoleOptions options)
        {
            Options = options ?? new PrettyConsoleOptions();
        }

        public PrettyConsole(IOptions<PrettyConsoleOptions> options)
        {
            Options = options.Value;
        }

        public PrettyConsole()
        {
        }

        public void Write(string text, bool prompt)
        {
            Write(text, Options.WriteColor, prompt);
        }

        public void Write(int value)
        {
            Write(value, Options.NumberColor);
        }

        public void Write(string text)
        {
            Write(text, Options.WriteColor);
        }

        public void Write(string text, ConsoleColor color, bool prompt)
        {
            Console.ForegroundColor = color;
            if (prompt)
            {
                Write(text, color);
                Console.WriteLine();
                Console.Write(Options.Prompt);
            }
            else
            {
                Console.Write(text);
            }
            Console.ResetColor();
        }

        public void Write(object value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public void Write(int value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public void Write(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
            var sb = new StringBuilder();
            sb.Append(value, times);
            Write(sb.ToString(), color);
        }

        public void WriteLine(string text, ConsoleColor color)
        {
            Write(text, color);
            Console.WriteLine();
        }

        public void WriteLine(string text)
        {
            Write(text, Options.WriteLineColor);
            Console.WriteLine();
        }

        public void WriteLine(object value)
        {
            Write(value, Options.WriteLineColor);
            Console.WriteLine();
        }

        public void WriteLine(object value, ConsoleColor color)
        {
            Write(value, color);
            Console.WriteLine();
        }

        public void WriteLine(int value)
        {
            Write(value, Options.NumberColor);
            Console.WriteLine();
        }

        public void WriteLine(char value, int times, ConsoleColor color = ConsoleColor.White)
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
        public void LogError(string message, Exception ex = null)
        {
            var sb = new StringBuilder($"[{DateTime.Now.ToShortTimeString()}]: ");
            sb.AppendLine(message);

            if (ex != null)
            {
                sb.AppendLine(ex.ToString());
            }

            WriteLine(sb.ToString(), ConsoleColor.Red);
        }

        public string ReadLine()
        {
            return Console.ReadLine() ?? "";
        }

        public void LogWarning(string message, Exception ex = null)
        {
            var sb = new StringBuilder($"[{DateTime.Now.ToShortTimeString()}]: ");
            sb.AppendLine(message);

            if (ex != null)
            {
                sb.AppendLine(ex.ToString());
            }

            WriteLine(sb.ToString(), ConsoleColor.Magenta);
        }

        public ConsoleKeyInfo ReadKey(bool dontShowKey = false)
        {
            return Console.ReadKey(dontShowKey);
        }
    }
}