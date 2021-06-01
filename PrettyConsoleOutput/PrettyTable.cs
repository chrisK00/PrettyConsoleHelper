using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrettyConsoleOutput
{
    public class PrettyTable
    {
        private readonly string[] _headers;
        private readonly IList<List<string>> _rows;
        private readonly string _columnSeparator;
        public ConsoleColor HeaderColor { get; set; }

        public PrettyTable(params string[] headers) : this(" | ", ConsoleColor.DarkYellow, headers)
        {
        }

        public PrettyTable(string columnSeparator = " | ", ConsoleColor headerColor = ConsoleColor.DarkYellow, params string[] headers)
        {
            _columnSeparator = columnSeparator;
            HeaderColor = headerColor;
            _headers = headers;
            _rows = new List<List<string>>();
        }

        /// <summary>
        /// Adds a new row
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddRow(params string[] row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row),"No items");
            if (row.Length > _headers.Length || row.Length < _headers.Length)
            {
                throw new ArgumentException($"Row items: {row.Length} has to match the length of headers: {_headers.Length}");
            }

            _rows.Add(row.ToList());
        }

        /// <summary>
        /// Outputs the table to the console
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Write()
        {
            if (!_rows.Any())
            {
                throw new InvalidOperationException("No rows");
            }

            var columnLengths = GetColumnLengths();
            var headers = GetFormattedHeaders(columnLengths);
            var header = string.Concat(headers);
            Console.Write($"{_columnSeparator.Trim()} ");
            PrettyConsole.WriteLine(header);
            PrettyConsole.WriteLine('-', header.Length + 1);

            var rows = GetFormattedRows(headers);
            var sb = new StringBuilder();

            for (int i = 0; i <= rows.Count; i += _headers.Length)
            {
                sb.AppendLine($"{_columnSeparator.Trim()} " + string.Concat(
                    rows.Take(_headers.Length)));

                sb.Append('-', header.Length + 1);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        private List<string> GetFormattedHeaders(List<int> columnLengths)
        {
            var headers = new List<string>();
            for (int i = 0; i < _headers.Length; i++)
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
                for (int i = 0; i < _headers.Length; i++)
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
    }
}