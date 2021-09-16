using System;
using System.Collections.Generic;

namespace PrettyConsoleHelper.Tests
{
    public class MockPrettyConsoleWithMultipleInputs : IPrettyConsole
    {
        public List<string> LinesToRead { get; set; } = new();
        public List<ConsoleKeyInfo> KeysToRead { get; set; } = new();

        public PrettyConsoleOptions Options { get; init; }

        public void LogError(string message, Exception ex = null)
        {
        }

        public void LogWarning(string message, Exception ex = null)
        {
        }

        public ConsoleKeyInfo ReadKey(bool dontShowKey)
        {
            var indexOfFirstItem = 0;
            var key = KeysToRead[indexOfFirstItem];
            KeysToRead.RemoveAt(indexOfFirstItem);
            return key;
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
}
