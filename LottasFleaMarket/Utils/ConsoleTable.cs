using System;
using System.Collections.Generic;

namespace LottasFleaMarket.Utils {
    public class ConsoleTable {
        public string RowFormat;
        public object[] HeaderRow { get; set; }
        public List<object[]> Rows = new List<object[]>();
        public string HeaderFormat = null;

        public ConsoleTable(string rowFormat, params object[] headers) {
            RowFormat = rowFormat;
            HeaderRow = headers;
        }

        public ConsoleTable WithHeaderFormat(string headerFormat) {
            HeaderFormat = headerFormat;
            return this;
        }

        public ConsoleTable PushRow(params object[] row) {
            Rows.Add(row);
            return this;
        }

        public void PrintTable() {
            Console.WriteLine(HeaderFormat ?? RowFormat, HeaderRow);
            foreach (var row in Rows) {
                Console.WriteLine(RowFormat, row);
            }
        }
    }
}