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
        public int GetIntInput(string message = "Enter a whole number", int minValue = int.MinValue, int maxValue = int.MaxValue)
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

        public bool Confirm(string message = "Enter (y/n)")
        {
            _console.Write(message, true);

            var input = Console.ReadLine().Trim().ToLower();

            if (input[0] == 'y')
            {
                return true;
            }
            return false;
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
                _console.WriteLine($"Enter a {enumType.Name}", _console.Options.PromptColor);
                _console.Write(sb.ToString(), true);

                if (Enum.TryParse(_console.ReadLine(), true, out TEnum result))
                {
                    return result;
                }
                _console.LogError($"Invalid input: enum must exist in: {enumType.Name}");
            }
        }

        /// <summary>
        /// Loops until the user has succesfully entered a datetime
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxDateTime"></param>
        /// <param name="minDateTime"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string message = "Enter a date", DateTime? minDateTime = null, DateTime? maxDateTime = null)
        {
            DateTime minDate = minDateTime ?? DateTime.MinValue;
            DateTime maxDate = maxDateTime ?? DateTime.MaxValue;

            if (maxDate < minDate)
            {
                throw new ArgumentException($"Maxvalue {maxDate} cannot be less than {minDate}");
            }

            while (true)
            {
                _console.Write(message, _console.Options.PromptColor, true);
                if (DateTime.TryParse(_console.ReadLine(), out DateTime dateTime) && dateTime <= maxDate && dateTime >= minDate)
                {
                    return dateTime;
                }

                _console.LogError($"Invalid input: Max value: {maxDate} Min value: {minDate}");
            }
        }

        public string Validate(string message = "Enter input", params ValidationAttribute[] validators)
        {
            if (validators.Length < 1)
            {
                throw new ArgumentException("You need at least 1 validator");
            }

            while (true)
            {
                _console.Write(message, _console.Options.PromptColor, true);
                var input = _console.ReadLine();
                var errorsSb = new StringBuilder();
                var hasErrors = false;
                foreach (var validator in validators)
                {
                    if (!validator.IsValid(input))
                    {
                        hasErrors = true;
                        errorsSb.Append(validator?.ErrorMessage);
                    }
                }

                if (hasErrors)
                {
                    _console.LogError($"Invalid input: {errorsSb}");
                    continue;
                }

                return input;
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
