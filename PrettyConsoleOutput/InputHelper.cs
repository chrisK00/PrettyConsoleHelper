using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrettyConsoleOutput
{
    public static class InputHelper
    {
        /// <summary>
        /// Loops until the user has succesfully entered a integer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static int GetIntInput(string message = "Enter a whole number: ", int maxValue = int.MaxValue, int minValue = int.MinValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentException($"Maxvalue {maxValue} cannot be less than {minValue}");
            }

            while (true)
            {
                PrettyConsole.Write(message);
                if (int.TryParse(Console.ReadLine(), out int input) && input <= maxValue && input >= minValue)
                {
                    return input;
                }

                PrettyConsole.LogError($"Invalid input: Max value: {maxValue} Min value: {minValue}");
            }
        }

        public static string Validate(ValidationAttribute validator, string message = "Enter input: ")
        {
            while (true)
            {
                PrettyConsole.Write(message);
                var input = Console.ReadLine();
                if (validator.IsValid(input))
                {
                    return input;
                }

                PrettyConsole.LogError($"Invalid input: {validator?.ErrorMessage}");
            }
        }

        /// <summary>
        /// Tries to parse the input to the specified type and make sure the specified validation is valid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <returns>If the type convertion is invalid and user chooses to exit the default value for T is returned</returns>
        public static T Validate<T>(ValidationAttribute validator, string message = "Enter input: ")
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            while (true)
            {
                PrettyConsole.Write(message);
                var input = Console.ReadLine();

                if (!converter.IsValid(input))
                {
                    PrettyConsole.WriteLine($"Invalid type convertion from: {input} to: {typeof(T)}", ConsoleColor.Red);
                    PrettyConsole.Write("Would you like to exit? (y/n): ");

                    if (Console.ReadLine().Trim().ToLower().StartsWith("y"))
                    {
                        return default;
                    }
                }
                else if (validator.IsValid(input))
                {
                    return (T)converter.ConvertFromString(input);
                }

                PrettyConsole.LogError($"Invalid input: {validator?.ErrorMessage}");
            }
        }
    }
}
