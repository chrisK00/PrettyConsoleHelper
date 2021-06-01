using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrettyConsoleOutput
{
    public class PrettyTable
    {
        private readonly IList<string> _headers = new List<string>();
        private readonly IList<List<string>> _rows;
        public string Separator { get; set; }
        public ConsoleColor HeaderColor { get; set; }

        public PrettyTable(params string[] headers) : this(" | ", ConsoleColor.DarkYellow, headers)
        {
        }

        public PrettyTable(string separator = " | ", ConsoleColor headerColor = ConsoleColor.DarkYellow, params string[] headers)
        {
            Separator = separator;
            HeaderColor = headerColor;
            _headers = headers;
            _rows = new List<List<string>>();
        }

        public void AddRow(List<string> row)
        {
            _ = row ?? throw new ArgumentNullException("No items");
            if (row.Count > _headers.Count)
            {
                throw new InvalidOperationException($"Row items: {row.Count} cannot be more than headers: {_headers.Count}");
            }

            _rows.Add(row);
        }

        public void Write()
        {
            if (!_rows.Any())
            {
                throw new InvalidOperationException("No rows");
            }

            var columnLengths = GetColumnLengths();
            var headers = GetFormattedHeaders(columnLengths);
            var header = string.Concat(headers);
            Console.Write("| ");
            PrettyConsole.WriteLine(header);
            PrettyConsole.WriteLine('-', header.Length + 1);

            var rows = GetFormattedRows(headers);
            var sb = new StringBuilder();

            for (int i = 0; i <= rows.Count; i += _headers.Count)
            {
                sb.AppendLine("| " + string.Concat(
                    rows.Take(_headers.Count)));

                sb.Append('-', header.Length + 1);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        private List<string> GetFormattedHeaders(List<int> columnLengths)
        {
            var headers = new List<string>();
            for (int i = 0; i < _headers.Count; i++)
            {
                headers.Add(_headers[i].PadRight(columnLengths[i]) + Separator);
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
                        row[i].PadRight(headers[i].Length - Separator.Length)
                        + Separator);
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