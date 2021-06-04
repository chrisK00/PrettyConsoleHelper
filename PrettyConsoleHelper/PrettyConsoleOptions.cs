using System;

namespace PrettyConsoleHelper
{
    public class PrettyConsoleOptions
    {
        public ConsoleColor WriteColor { get; }
        public ConsoleColor WriteLineColor { get; }
        public ConsoleColor NumberColor { get; }
        public ConsoleColor PromptColor { get; }
        public string Prompt { get; }

        public PrettyConsoleOptions(ConsoleColor writeColor = ConsoleColor.Yellow, ConsoleColor writeLineColor = ConsoleColor.Cyan,
            ConsoleColor numberColor = ConsoleColor.Cyan, string prompt = " > ",
            ConsoleColor promptColor = ConsoleColor.DarkYellow)
        {
            WriteColor = writeColor;
            WriteLineColor = writeLineColor;
            NumberColor = numberColor;
            PromptColor = promptColor;
            Prompt = prompt;
        }

        public PrettyConsoleOptions() : this(ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Cyan, "> ",
            ConsoleColor.DarkYellow)
        {
        }

        public PrettyConsoleOptions(ConsoleColor writeColor, ConsoleColor writeLineColor, string prompt, ConsoleColor promptColor)
            : this(writeColor, writeLineColor, ConsoleColor.Cyan, prompt, promptColor)
        {
        }

        public PrettyConsoleOptions(ConsoleColor writeColor, ConsoleColor writeLineColor)
            : this(writeColor, writeLineColor, ConsoleColor.Cyan, "> ", ConsoleColor.DarkYellow)
        {
            WriteColor = writeColor;
            WriteLineColor = writeLineColor;
        }

        public PrettyConsoleOptions(string prompt, ConsoleColor promptColor)
            : this(ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Cyan, prompt, promptColor)
        {
        }
    }
}