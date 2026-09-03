namespace Premya.Api.Application.Imports;

public record ParsedExcel(
    IReadOnlyList<ParsedColumn> Columns,
    IReadOnlyList<IReadOnlyList<object?>> Rows);

public record ParsedColumn(string Name, string DataType);
