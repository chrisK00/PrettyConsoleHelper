using System;

namespace PrettyConsoleHelper
{
    public interface IPrettyConsole
    {
        public static IPrettyConsole Console = new PrettyConsole();
        PrettyConsoleOptions Options { get; init; }
        void Write(char value, int times, ConsoleColor color = ConsoleColor.White);
        ConsoleKeyInfo ReadKey(bool dontShowKey);
        void Write(object value, ConsoleColor color);
        void Write(string text);

        /// <summary>
        /// Writes text to console using default console color
        /// </summary>
        /// <param name="text"></param>
        /// <param name="prompt"></param>
        void Write(string text, bool prompt);
        void Write(int value, ConsoleColor color);
        void Write(string text, ConsoleColor color, bool prompt);
        void WriteLine(object value);
        void WriteLine(object value, ConsoleColor color);

        /// <summary>
        /// Writes text to console using default console color
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(string text);
        void WriteLine(string text, ConsoleColor color);
        void WriteLine(char value, int times, ConsoleColor color = ConsoleColor.White);

        /// <summary>
        /// Logs an error to the console: [time]: message: error
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        void LogError(string message, Exception ex = null);
        void LogWarning(string message, Exception ex = null);
        string ReadLine();
        void Write(int value);
    }
}
