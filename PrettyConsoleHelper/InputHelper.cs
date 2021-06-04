using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrettyConsoleHelper
{
    public class InputHelper
    {
        private readonly IPrettyConsole _console;

        public InputHelper(IPrettyConsole console)
        {
            _console = console;
        }

        public InputHelper()
        {
            _console = PrettyStatic.ConsolePretty;
        }

        /// <summary>
        /// Loops until the user has succesfully entered a integer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public int GetIntInput(string message = "Enter a whole number", int maxValue = int.MaxValue, int minValue = int.MinValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentException($"Maxvalue {maxValue} cannot be less than {minValue}");
            }

            while (true)
            {
                _console.Write(message, _console.Options.PromptColor, true);
                if (int.TryParse(_console.ReadLine(), out int input) && input <= maxValue && input >= minValue)
                {
                    return input;
                }

                _console.LogError($"Invalid input: Max value: {maxValue} Min value: {minValue}");
            }
        }

        /// <summary>
        ///  Loops until the user has succesfully entered a enum that exists inside the Generic enum type. Case insensetive
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="message"></param>
        /// <returns>Enum of the specified Type</returns>
        public TEnum GetEnumInput<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Not an enum");
            }

            var sb = new StringBuilder();
            foreach (var item in enumType.GetEnumNames())
            {
                sb.AppendLine(item);
            }

            while (true)
            {
                _console.Write($"Enter a {enumType.Name}", _console.Options.PromptColor);
                _console.Write(sb.ToString(), _console.Options.PromptColor, true);

                if (Enum.TryParse(_console.ReadLine(), true, out TEnum result))
                {
                    return result;
                }
                _console.LogError($"Invalid input: enum must exist in: {enumType.Name}");
            }
        }

        public string Validate(ValidationAttribute validator, string message = "Enter input")
        {
            while (true)
            {
                _console.Write(message, _console.Options.PromptColor, true);
                var input = _console.ReadLine();
                if (validator.IsValid(input))
                {
                    return input;
                }

                _console.LogError($"Invalid input: {validator?.ErrorMessage}");
            }
        }

        /// <summary>
        /// Tries to parse the input to the specified type and make sure the specified validation is valid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <returns>If the type convertion is invalid and user chooses to exit the default value for T is returned</returns>
        public T Validate<T>(ValidationAttribute validator, string message = "Enter input")
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            while (true)
            {
                _console.Write(message, _console.Options.PromptColor, true);
                var input = _console.ReadLine();

                if (!converter.IsValid(input))
                {
                    _console.LogError($"Invalid type convertion from: {input} to: {typeof(T)}");
                    _console.Write("Would you like to exit? (y/n)", _console.Options.PromptColor, true);                         

                    if (_console.ReadLine().Trim().ToLower().StartsWith("y"))
                    {
                        return default;
                    }
                }
                else if (validator.IsValid(input))
                {
                    return (T)converter.ConvertFromString(input);
                }
                else
                {
                    _console.LogError($"Invalid input: {validator?.ErrorMessage}");
                }
            }
        }
    }
}
