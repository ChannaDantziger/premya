using Microsoft.EntityFrameworkCore;
using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.Imports;
using Premya.Api.Infrastructure.Persistence;

namespace Premya.Api.Infrastructure.Repositories;

public class DynamicDataRepository(PremyaDbContext dbContext) : IDynamicDataRepository
{
    public async Task<ImportDataResponse?> GetAsync(
        int metricId,
        int? importBatchId,
        string? fieldName,
        string? search,
        string? sortBy,
        bool descending,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 200);
        var batch = await dbContext.ImportBatches.AsNoTracking()
            .Where(item => item.MetricId == metricId && (!importBatchId.HasValue || item.Id == importBatchId.Value))
            .OrderByDescending(item => item.ImportedAt)
            .FirstOrDefaultAsync(cancellationToken);
        if (batch is null) return null;

        var fields = await dbContext.FileStructureFields.AsNoTracking()
            .Where(field => field.FileStructureVersionId == batch.FileStructureVersionId && field.IsRelevant)
            .OrderBy(field => field.DisplayOrder)
            .ToListAsync(cancellationToken);
        var records = await dbContext.DynamicRecords.AsNoTracking()
            .Include(record => record.DynamicValues)
            .ThenInclude(value => value.FileStructureField)
            .Where(record => record.ImportBatchId == batch.Id)
            .OrderBy(record => record.RowNumber)
            .ToListAsync(cancellationToken);

        var mapped = records.Select(record => (IReadOnlyDictionary<string, object?>)fields.ToDictionary(
            field => field.FieldName,
            field => GetValue(record.DynamicValues.FirstOrDefault(value => value.FileStructureFieldId == field.Id)))).ToList();
        if (!string.IsNullOrWhiteSpace(fieldName) && !string.IsNullOrWhiteSpace(search))
        {
            mapped = mapped.Where(record => record.TryGetValue(fieldName, out var value) &&
                value?.ToString()?.Contains(search, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }

        if (!string.IsNullOrWhiteSpace(sortBy) && fields.Any(field => field.FieldName == sortBy))
        {
            mapped = descending
                ? mapped.OrderByDescending(record => record.GetValueOrDefault(sortBy!)).ToList()
                : mapped.OrderBy(record => record.GetValueOrDefault(sortBy!)).ToList();
        }

        var total = mapped.Count;
        var paged = mapped.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return new ImportDataResponse(fields.Select(field => field.FieldName).ToList(), paged, page, pageSize, total);
    }

    private static object? GetValue(Domain.Entities.DynamicValue? value)
    {
        if (value is null) return null;
        if (value.ValueText is not null) return value.ValueText;
        if (value.ValueNumber is not null) return value.ValueNumber;
        if (value.ValueDate is not null) return value.ValueDate;
        return value.ValueBoolean;
    }
}
