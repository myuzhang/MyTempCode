using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinqToExcel;
using LinqToExcel.Attributes;

namespace Utility
{
    public class ExcelReader
    {
        private readonly ExcelQueryFactory _book;

        private readonly string _excelFile;

        public ExcelReader(string excelFile)
        {
            _book = new ExcelQueryFactory(excelFile)
            {
                //DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                //UsePersistentConnection = true,
                ReadOnly = true
            };
            _excelFile = excelFile;
        }

        public static ExcelReader GetExcelReader(string excelFile) => new ExcelReader(excelFile);

        public List<Row> GetAllRowsBySheet(string sheet = null)
        {
            IQueryable<Row> all = QueryRowsBySheet(sheet);
            return all.ToList();
        }

        public IQueryable<Row> QueryRowsBySheet(string sheet = null)
        {
            return from a in _book.Worksheet(sheet ?? Path.GetFileNameWithoutExtension(_excelFile)) select a;
        }

        public IQueryable<T> QueryRowsBySheet<T>(string sheet = null) where T:class
        {
            return from a in _book.Worksheet<T>(sheet ?? Path.GetFileNameWithoutExtension(_excelFile)) select a;
        }

        // read two columns from CSV file and make into dictionary - this one doesn't handle special words other than English - FIXME:
        public Dictionary<string, string> ReadCsv(string csvFile)
        {
            return File.ReadLines(csvFile)
                .Where(line => !string.IsNullOrWhiteSpace(line) && line[0] != '!')
                .Select(line => line.Split(','))
                .ToDictionary(data => data[0].Trim(), data => data.Length > 1 ? data[1].Trim() : string.Empty);
        }
    }

    /// <summary>
    /// To serialize to a class.
    /// </summary>
    public class SerializeExample
    {
        // the column name
        [ExcelColumn("ColumnName")]
        public string SurgeonName { get; set; }

        // if and only if no column name, column number being used
        [ExcelColumn("F20")]
        public int PreferF { get; set; }

        // no attribute, no serializing
        public bool DefaultSurgeon { get; set; }
    }
}
