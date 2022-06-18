using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrettyConsoleHelper
{
    public interface IInputHelper
    {
        public static IInputHelper Input = new InputHelper(IPrettyConsole.Console);
        bool Confirm(string message = "Enter (y/n)");
        DateTime GetDateTime(string message = "Enter a date", DateTime? minDateTime = null, DateTime? maxDateTime = null);
        TEnum GetEnumInput<TEnum>() where TEnum : struct;
        int GetIntInput(string message = "Enter a whole number", int minValue = int.MinValue, int maxValue = int.MaxValue);
        Dictionary<string, string> ParseOptions(string[] options, string[] inputs, string prefixToRemove = null);
        void PrintGenericTypeList<T>(IEnumerable<T> items, string message = null) where T : class;
        T Select<T>(IList<T> items, string message = null, Func<T, object> format = null) where T : class;
        T Select<T>(IList<T> items, PrettyTable table) where T : class;
        TEnum Select<TEnum>(string message = null) where TEnum : struct;
        string Validate(string message = "Enter input", params ValidationAttribute[] validators);
        string Validate(ValidationAttribute validator, string message = "Enter input");
        T Validate<T>(ValidationAttribute validator, string message = "Enter input");
    }
}