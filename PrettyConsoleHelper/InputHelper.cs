using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PrettyConsoleHelper
{
    public class InputHelper : IInputHelper
    {
        private readonly IPrettyConsole _console;

        public InputHelper(IPrettyConsole console)
        {
            _console = console;
        }

        public InputHelper()
        {
            _console = IPrettyConsole.Console;
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

            var input = _console.ReadLine().Trim().ToLower();

            return input[0] == 'y';
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

        /// <summary>
        /// Gets the values from the input string out of a choosen set of options
        /// Use .TryGetValue(nameof(Class.PropertyName)) on the returned result
        /// </summary>
        /// <param name="options"></param>
        /// <param name="inputs"></param>
        /// <param name="prefixToRemove"></param>
        /// <returns>The option ToTitleCase if a prefix will be removed and its corresponding value in a dictionary.</returns>
        public Dictionary<string, string> ParseOptions(string[] options, string[] inputs, string prefixToRemove = null)
        {
            var optionsValues = new Dictionary<string, string>();

            var optionIndexes = options
                .Where(x => inputs.Contains(x))
                .Select((value) => (value, index: Array.IndexOf(inputs, value)))
                .OrderBy(x => x.index).ToArray();

            for (int i = 0; i < optionIndexes.Length; i++)
            {
                var valueStartIndex = 0;
                var valueEndIndex = 0;
                var nextIndex = i + 1;

                if (nextIndex == optionIndexes.Length) //last option
                {
                    valueStartIndex = optionIndexes[i].index + 1; //value comes after the option
                    valueEndIndex = inputs.Length;
                }
                else
                {
                    valueStartIndex = optionIndexes[i].index + 1;
                    valueEndIndex = optionIndexes[nextIndex].index;
                }

                var values = inputs[valueStartIndex..valueEndIndex];

                if (values.Length < 1) //no value inputted
                {
                    continue;
                }

                var value = string.Join(' ', values);

                if (!string.IsNullOrWhiteSpace(prefixToRemove))
                {
                    var formattedOption = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(optionIndexes[i].value.Replace(prefixToRemove, string.Empty));
                    optionsValues.Add(formattedOption, value);
                }
                else
                {
                    optionsValues.Add(optionIndexes[i].value, value);
                }
            }

            return optionsValues;
        }

        public void PrintGenericTypeList<T>(IEnumerable<T> items, string message = null) where T : class
        {
            var type = typeof(T);
            var propertyInfos = type.GetProperties();

            var sb = new StringBuilder().AppendLine(message ?? $"Select a {type.Name}");

            foreach (var item in items)
            {
                foreach (var property in propertyInfos)
                {
                    sb.Append(' ').Append(property.GetValue(item)).Append(' ');
                }
                sb.AppendLine();
            }

            _console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Make sure to add items to the table before calling this method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="table"></param>
        /// <returns>The selected item of type <typeparamref name="T"/></returns>
        public T Select<T>(IList<T> items, PrettyTable table) where T : class
        {
            if (table == null || table.RowCount < 1)
            {
                return null;
            }

            Console.Clear();
            var (Left, Top) = Console.GetCursorPosition();
            var topMargin = 2;
            Left = 2;
            Top += topMargin;

            table.Write();

            var count = items.Count;
            var currentIndex = 0;
            var key = ConsoleKey.Zoom;

            while (key != ConsoleKey.Enter)
            {
                Console.SetCursorPosition(Left, Top);
                key = _console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.DownArrow or ConsoleKey.S:
                        if ((currentIndex + 1) == count) //last item
                        {
                            currentIndex = 0;
                            Top = topMargin;
                        }
                        else
                        {
                            currentIndex++;
                            Top += topMargin;
                        }
                        break;
                    case ConsoleKey.UpArrow or ConsoleKey.W:
                        if ((currentIndex - 1) < 0) //first item
                        {
                            currentIndex = count - 1;
                            Top = count * topMargin;
                        }
                        else
                        {
                            currentIndex--;
                            Top -= topMargin;
                        }
                        break;
                }
            }

            Console.Clear();
            return items[currentIndex];
        }

        private (int LeftPos, int TopPos) PrepareSelect()
        {
            Console.Clear();
            var cursorPos = Console.GetCursorPosition();
            cursorPos.Left++;
            cursorPos.Top++;
            return cursorPos;
        }

        private void PrintFormattedList<T>(IList<T> items, Func<T, object> format, string message = null)
        {
            var formattedItems = items.Select(format).ToArray();
            var sb = new StringBuilder().AppendLine(message ?? $"Select a {typeof(T).Name}");

            foreach (var item in formattedItems)
            {
                sb.Append(' ').Append(item).Append(' ').AppendLine();
            }

            _console.WriteLine(sb.ToString());
        }

        public T Select<T>(IList<T> items, string message = null, Func<T, object> format = null) where T : class
        {
            if (items.Count < 1)
            {
                return null;
            }

            var (Left, Top) = PrepareSelect();

            if (format is null)
                PrintGenericTypeList(items, message);
            else
                PrintFormattedList(items, format, message);

            var selectedIndex = HandleSelect(Left, Top, items.Count);

            return items[selectedIndex];
        }

        public TEnum Select<TEnum>(string message = null) where TEnum : struct
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum) throw new ArgumentException("Not an enum");

            var (Left, Top) = PrepareSelect();

            var sb = new StringBuilder();
            var enumNames = enumType.GetEnumNames();

            foreach (var item in enumNames)
                sb.Append(' ').AppendLine(item);

            _console.WriteLine(message ?? $"Select a {enumType.Name}");
            _console.WriteLine(sb.ToString());

            var selectedIndex = HandleSelect(Left, Top, enumNames.Length);

            return Enum.Parse<TEnum>(enumNames[selectedIndex]);
        }

        private int HandleSelect(int left, int top, int count)
        {
            var key = ConsoleKey.Zoom;
            var currentIndex = 0;

            while (key != ConsoleKey.Enter)
            {
                Console.SetCursorPosition(left, top);
                key = _console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.DownArrow or ConsoleKey.S:
                        if ((currentIndex + 1) == count) //last item
                        {
                            currentIndex = 0;
                            top = 1;
                        }
                        else
                        {
                            currentIndex++;
                            top++;
                        }
                        break;
                    case ConsoleKey.UpArrow or ConsoleKey.W:
                        if ((currentIndex - 1) < 0) //first item
                        {
                            currentIndex = count - 1;
                            top = count;
                        }
                        else
                        {
                            currentIndex--;
                            top--;
                        }
                        break;
                }
            }

            Console.Clear();

            return currentIndex;
        }
    }
}
