using LinqToExcel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharedService
{
    public class CsvReader
    {
        private readonly ExcelQueryFactory _book;

        private readonly string _excelFile;

        public CsvReader(string excelFile)
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

        public static CsvReader GetExcelReader(string excelFile) => new CsvReader(excelFile);

        public List<Row> GetAllRowsBySheet(string sheet = null)
        {
            IQueryable<Row> all = QueryRowsBySheet(sheet);
            return all.ToList();
        }

        public IQueryable<Row> QueryRowsBySheet(string sheet = null)
        {
            return from a in _book.Worksheet(sheet ?? Path.GetFileNameWithoutExtension(_excelFile)) select a;
        }

        public Dictionary<string, string> ToDictionary()
        {
            return GetAllRowsBySheet()
                .Where(r => !string.IsNullOrWhiteSpace(r[0].Cast<string>()) && !r[0].Cast<string>().StartsWith("!"))
                .ToDictionary(r => r[0].ToString().Trim(), r => r[1]?.Value.ToString().Trim() ?? string.Empty);
        }
    }
}
