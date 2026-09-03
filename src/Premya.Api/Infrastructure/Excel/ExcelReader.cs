using ExcelDataReader;
using Premya.Api.Application.Imports;
using Premya.Api.Application.Interfaces;

namespace Premya.Api.Infrastructure.Excel;

public class ExcelReader : IExcelReader
{
    private const string DefaultDataType = "Text";

    public Task<ParsedExcel> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var dataSet = reader.AsDataSet();
        var table = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
        if (table is null || table.Columns.Count == 0 || table.Rows.Count == 0)
        {
            throw new InvalidDataException("The Excel file must contain a header row and at least one data row.");
        }

        var names = table.Rows[0].ItemArray
            .Select((value, index) => string.IsNullOrWhiteSpace(value?.ToString()) ? $"Column{index + 1}" : value.ToString()!.Trim())
            .ToList();
        var columns = names.Select((name, index) => new ParsedColumn(name, InferDataType(table, index))).ToList();
        var rows = table.Rows.Cast<System.Data.DataRow>()
            .Skip(1)
            .Select(row => (IReadOnlyList<object?>)row.ItemArray.Select(NormalizeValue).ToList())
            .ToList();

        return Task.FromResult(new ParsedExcel(columns, rows));
    }

    private static string InferDataType(System.Data.DataTable table, int columnIndex)
    {
        var values = table.Rows.Cast<System.Data.DataRow>()
            .Skip(1)
            .Select(row => NormalizeValue(row[columnIndex]))
            .Where(value => value is not null)
            .ToList();

        if (values.Count == 0) return DefaultDataType;
        if (values.All(value => value is bool)) return "Boolean";
        if (values.All(value => value is DateTime)) return "Date";
        if (values.All(value => value is byte or short or int or long)) return "Integer";
        if (values.All(value => value is decimal or double or float or byte or short or int or long)) return "Decimal";
        return DefaultDataType;
    }

    private static object? NormalizeValue(object? value) =>
        value is null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) ? null : value;
}
