using System;

namespace PrettyConsoleHelper
{
    public class PrettyConsoleOptions
    {
        public ConsoleColor WriteColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor WriteLineColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor NumberColor { get; set; } = ConsoleColor.DarkGreen;
        public ConsoleColor PromptColor { get; set; } = ConsoleColor.DarkYellow;
        public string Prompt { get; set; } =" >> ";
    }
}