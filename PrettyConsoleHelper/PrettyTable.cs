using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrettyConsoleHelper
{
    public class PrettyTable
    {
        private readonly List<string> _headers;
        private readonly IList<List<string>> _rows;
        private readonly string _columnSeparator;
        public int RowCount => _rows.Count;
        public ConsoleColor HeaderColor { get; set; }

        public PrettyTable(params string[] headers) : this(" | ", ConsoleColor.DarkYellow, headers)
        {
        }

        public PrettyTable(string columnSeparator = " | ", ConsoleColor headerColor = ConsoleColor.DarkYellow, params string[] headers)
        {
            _columnSeparator = columnSeparator;
            HeaderColor = headerColor;
            _headers = new List<string>(headers);
            _rows = new List<List<string>>();
        }

        public string Headers => string.Join(',', _headers);

        private List<string> GetFormattedHeaders(List<int> columnLengths)
        {
            var headers = new List<string>();
            for (int i = 0; i < _headers.Count; i++)
            {
                headers.Add(_headers[i].PadRight(columnLengths[i]) + _columnSeparator);
            }
            return headers;
        }

        private List<string> GetFormattedRows(List<string> headers)
        {
            var rows = new List<string>();

            foreach (var row in _rows)
            {
                for (int i = 0; i < _headers.Count; i++)
                {
                    rows.Add(
                        row[i].PadRight(headers[i].Length - _columnSeparator.Length)
                        + _columnSeparator);
                }
            }
            return rows;
        }

        private List<int> GetColumnLengths()
        {
            return _headers.Select((header, i) =>
           _rows.Select(row => row[i].Length)
           .Max()).ToList();
        }

        /// <summary>
        /// Adds a header to the existing headers if there are no rows
        /// </summary>
        /// <param name="header"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public PrettyTable AddHeader(string header)
        {
            if (_rows.Any())
            {
                throw new InvalidOperationException("You can only add headers while there are no rows");
            }

            _headers.Add(header);
            return this;
        }

        public PrettyTable AddDefaultHeaders<T>() where T : class
        {
            if (_rows.Any())
            {
                throw new InvalidOperationException("You can only add headers while there are no rows");
            }

            var properties = typeof(T).GetProperties();

            _headers.AddRange(properties.Select(x => x.Name));

            return this;
        }

        /// <summary>
        /// Adds headers
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public PrettyTable AddHeaders(params string[] headers)
        {
            if (_rows.Any())
            {
                throw new InvalidOperationException("You can only add headers while there are no rows");
            }
            _ = headers ?? throw new ArgumentNullException(nameof(headers), "No items");

            _headers.AddRange(headers.Select(x => $"{x}"));
            return this;
        }

        /// <summary>
        /// Adds a new row
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddRow(params object[] row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row), "No items");
            if (row.Length > _headers.Count || row.Length < _headers.Count)
            {
                throw new ArgumentException($"Row items: {row.Length} has to match the length of headers: {_headers.Count}");
            }

            _rows.Add(row.Select(x => $"{x}").ToList());
        }

        public PrettyTable AddRows<T>(IEnumerable<T> rows) where T : class
        {
            if (!rows.Any())
            {
                throw new ArgumentNullException(nameof(rows), "No items");
            }

            var properties = typeof(T).GetProperties();

            if (properties.Length > _headers.Count || properties.Length < _headers.Count)
            {
                throw new ArgumentException($"Row items: {properties.Length} has to match the length of headers: {_headers.Count}");
            }

            foreach (var row in rows)
            {
                var values = properties.Select(x => x.GetValue(row));
                _rows.Add(values.Select(x => $"{x}").ToList());
            }

            return this;
        }

        /// <summary>
        /// Adds rows and headers using the generic class's property names
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public PrettyTable AddRowsWithDefaultHeaders<T>(IEnumerable<T> rows) where T : class
        {
            if (!rows.Any() || _headers.Any())
            {
                throw new ArgumentNullException(nameof(rows), "No items or headers are already defined");
            }

            var properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                _headers.Add(item.Name);
            }

            foreach (var row in rows)
            {
                var values = properties.Select(x => x.GetValue(row));
                _rows.Add(values.Select(x => $"{x}").ToList());
            }

            return this;
        }

        /// <summary>
        /// Removes the headers and rows
        /// </summary>
        public void ResetTable()
        {
            _headers.Clear();
            _rows.Clear();
        }

        /// <summary>
        /// Outputs the table to the console
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public PrettyTable Write()
        {
            if (!_rows.Any())
            {
                throw new InvalidOperationException("No rows");
            }

            var columnLengths = GetColumnLengths();
            var headers = GetFormattedHeaders(columnLengths);
            var header = string.Concat(headers);
            IPrettyConsole.Console.Write($"{_columnSeparator.Trim()} ", HeaderColor);
            IPrettyConsole.Console.WriteLine(header, HeaderColor);
            IPrettyConsole.Console.WriteLine('-', header.Length + 1);

            var rows = GetFormattedRows(headers);
            var sb = new StringBuilder();

            for (int i = 0; i < rows.Count; i += _headers.Count)
            {
                sb.AppendLine($"{_columnSeparator.Trim()} " + string.Concat(
                    rows.Skip(i).Take(_headers.Count)));

                sb.Append('-', header.Length + 1);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
            return this;
        }
    }
}