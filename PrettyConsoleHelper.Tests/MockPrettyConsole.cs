using System;
using System.Collections.Generic;

namespace PrettyConsoleHelper.Tests
{
    public class MockPrettyConsoleWithMultipleInputs : IPrettyConsole
    {
        public List<string> LinesToRead { get; set; } = new();

        public PrettyConsoleOptions Options { get; init; }

        public void LogError(string message, Exception ex = null)
        {
        }

        public void LogWarning(string message, Exception ex = null)
        {
        }

        public string ReadLine()
        {
            var indexOfFirstItem = 0;
            var line = LinesToRead[indexOfFirstItem];
            LinesToRead.RemoveAt(indexOfFirstItem);
            return line;
        }

        public void Write(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
        }

        public void Write(object value, ConsoleColor color)
        {
        }

        public void Write(string text)
        {
        }

        public void Write(string text, bool prompt)
        {
        }

        public void Write(int value, ConsoleColor color)
        {
        }

        public void Write(string text, ConsoleColor color, bool prompt)
        {
        }

        public void Write(int value)
        {
        }

        public void WriteLine(object value)
        {
        }

        public void WriteLine(object value, ConsoleColor color)
        {
        }

        public void WriteLine(string text)
        {
        }

        public void WriteLine(string text, ConsoleColor color)
        {
        }

        public void WriteLine(char value, int times, ConsoleColor color = ConsoleColor.White)
        {
        }
    }

    public class MockPrettyConsole : IPrettyConsole
    {
        public MockPrettyConsole()
        {
            Options = new PrettyConsoleOptions();
        }

        public PrettyConsoleOptions Options { get; init; }
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

        public void Write(string text)
        {
            throw new NotImplementedException();
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
