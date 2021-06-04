using System;

namespace PrettyConsoleHelper.Tests
{
    public class MockPrettyConsole : IPrettyConsole
    {
        public MockPrettyConsole()
        {
            Options = new PrettyConsoleOptions();
        }

        public PrettyConsoleOptions Options { get; set; }
        public string ReturnValue { get; set; }

        public void LogError(string message, Exception ex = null)
        {
            return;
        }

        public void LogWarning(string message, Exception ex = null)
        {
            return;
        }

        public string ReadLine()
        {
            return ReturnValue;
        }

        public void Write(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
            return;
        }

        public void Write(object value, ConsoleColor color)
        {
            return;
        }

        public void Write(string text, bool prompt)
        {
            return;
        }

        public void Write(int value, ConsoleColor color)
        {
            return;
        }

        public void Write(string text, ConsoleColor color, bool prompt)
        {
            return;

        }

        public void Write(int value)
        {
            return;
        }

        public void WriteLine(object value)
        {
            return;
        }

        public void WriteLine(object value, ConsoleColor color)
        {
            return;
        }

        public void WriteLine(string text)
        {
            return;
        }

        public void WriteLine(string text, ConsoleColor color)
        {
            return;
        }

        public void WriteLine(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
            return;
        }
    }
}
