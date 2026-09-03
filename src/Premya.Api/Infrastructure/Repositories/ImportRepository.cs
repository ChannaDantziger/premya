using Microsoft.EntityFrameworkCore;
using Premya.Api.Application.Imports;
using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.Imports;
using Premya.Api.Domain.Entities;
using Premya.Api.Infrastructure.Persistence;

namespace Premya.Api.Infrastructure.Repositories;

public class ImportRepository(PremyaDbContext dbContext) : IImportRepository
{
    public async Task<IReadOnlyList<ImportResponse>> GetHistoryAsync(int metricId, CancellationToken cancellationToken) =>
        await dbContext.ImportBatches.AsNoTracking()
            .Where(batch => batch.MetricId == metricId)
            .OrderByDescending(batch => batch.ImportedAt)
            .Select(batch => new ImportResponse(
                batch.Id,
                batch.MetricId,
                batch.FileStructureVersionId,
                batch.FileName,
                batch.Status,
                batch.RecordCount,
                batch.ErrorMessage))
            .ToListAsync(cancellationToken);

    public Task<ImportResponse?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.ImportBatches.AsNoTracking()
            .Where(batch => batch.Id == id)
            .Select(batch => new ImportResponse(
                batch.Id,
                batch.MetricId,
                batch.FileStructureVersionId,
                batch.FileName,
                batch.Status,
                batch.RecordCount,
                batch.ErrorMessage))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<ImportResponse?> ImportAsync(
        int metricId,
        string fileName,
        int dataYear,
        string calculationPeriod,
        ParsedExcel parsedExcel,
        CancellationToken cancellationToken)
    {
        var metric = await dbContext.Metrics
            .Include(item => item.MetricFields)
            .Include(item => item.FileStructureVersions)
            .ThenInclude(item => item.Fields)
            .FirstOrDefaultAsync(item => item.Id == metricId, cancellationToken);
        if (metric is null || !string.Equals(metric.SourceType, "Excel", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var structure = FindMatchingStructure(metric, parsedExcel.Columns);
        if (structure is null)
        {
            foreach (var version in metric.FileStructureVersions) version.IsActive = false;
            var nextVersion = metric.FileStructureVersions.Count == 0
                ? 1
                : metric.FileStructureVersions.Max(item => item.VersionNumber) + 1;
            structure = new FileStructureVersion
            {
                MetricId = metricId,
                VersionNumber = nextVersion,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            structure.Fields = parsedExcel.Columns.Select((column, index) => new FileStructureField
            {
                FieldName = column.Name,
                DataType = column.DataType,
                MetricFieldId = metric.MetricFields.FirstOrDefault(field =>
                    string.Equals(field.FieldName, column.Name, StringComparison.OrdinalIgnoreCase))?.Id,
                IsRelevant = metric.MetricFields.FirstOrDefault(field =>
                    string.Equals(field.FieldName, column.Name, StringComparison.OrdinalIgnoreCase))?.IsRelevant ?? true,
                DisplayOrder = index
            }).ToList();
            dbContext.FileStructureVersions.Add(structure);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var batch = new ImportBatch
        {
            MetricId = metricId,
            FileStructureVersionId = structure.Id,
            FileName = fileName,
            DataYear = dataYear,
            CalculationPeriod = calculationPeriod,
            ImportedAt = DateTime.UtcNow,
            Status = "Pending"
        };
        dbContext.ImportBatches.Add(batch);
        await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            foreach (var row in parsedExcel.Rows.Select((values, index) => new { values, index }))
            {
                var record = new DynamicRecord { ImportBatchId = batch.Id, RowNumber = row.index + 2 };
                record.DynamicValues = structure.Fields.Select((field, index) =>
                {
                    var value = index < row.values.Count ? row.values[index] : null;
                    return ToDynamicValue(record, field, value);
                }).ToList();
                dbContext.DynamicRecords.Add(record);
            }

            batch.RecordCount = parsedExcel.Rows.Count;
            batch.Status = "Succeeded";
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception) when (exception is FormatException or InvalidDataException)
        {
            batch.Status = "Failed";
            batch.ErrorMessage = exception.Message;
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        return new ImportResponse(batch.Id, batch.MetricId, batch.FileStructureVersionId,
            batch.FileName, batch.Status, batch.RecordCount, batch.ErrorMessage);
    }

    private static FileStructureVersion? FindMatchingStructure(Metric metric, IReadOnlyList<ParsedColumn> columns) =>
        metric.FileStructureVersions.FirstOrDefault(version =>
            version.Fields.Count == columns.Count && version.Fields
                .OrderBy(field => field.DisplayOrder)
                .Select((field, index) => field.FieldName == columns[index].Name && field.DataType == columns[index].DataType)
                .All(matches => matches));

    private static DynamicValue ToDynamicValue(DynamicRecord record, FileStructureField field, object? value)
    {
        var result = new DynamicValue { DynamicRecord = record, FileStructureFieldId = field.Id };
        if (value is null) return result;
        switch (field.DataType)
        {
            case "Integer": result.ValueNumber = Convert.ToInt64(value); break;
            case "Decimal": result.ValueNumber = Convert.ToDecimal(value); break;
            case "Date": result.ValueDate = Convert.ToDateTime(value); break;
            case "Boolean": result.ValueBoolean = Convert.ToBoolean(value); break;
            default: result.ValueText = value.ToString(); break;
        }
        return result;
    }
}
