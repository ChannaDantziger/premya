using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.MetricFields;
using Premya.Api.Domain.Entities;

namespace Premya.Api.Application.Services;

public class MetricFieldService(IMetricFieldRepository repository) : IMetricFieldService
{
    public async Task<IReadOnlyList<MetricFieldResponse>> GetAllAsync(int metricId, CancellationToken cancellationToken) =>
        (await repository.GetAllAsync(metricId, cancellationToken)).Select(Map).ToList();

    public async Task<(MetricFieldResponse? Result, string? Error)> CreateAsync(
        int metricId,
        CreateMetricFieldRequest request,
        CancellationToken cancellationToken)
    {
        if (!await repository.MetricExistsAsync(metricId, cancellationToken))
        {
            return (null, "Metric was not found.");
        }

        if (await repository.ExistsByNameAsync(metricId, request.FieldName, null, cancellationToken))
        {
            return (null, "A field with this name already exists for the metric.");
        }

        var field = new MetricField
        {
            MetricId = metricId,
            FieldName = request.FieldName,
            DataType = request.DataType,
            IsRelevant = request.IsRelevant,
            DisplayOrder = request.DisplayOrder
        };

        await repository.AddAsync(field, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return (Map(field), null);
    }

    public async Task<(MetricFieldResponse? Result, string? Error)> UpdateAsync(
        int id,
        UpdateMetricFieldRequest request,
        CancellationToken cancellationToken)
    {
        var field = await repository.GetByIdAsync(id, cancellationToken);
        if (field is null)
        {
            return (null, null);
        }

        if (await repository.ExistsByNameAsync(field.MetricId, request.FieldName, id, cancellationToken))
        {
            return (null, "A field with this name already exists for the metric.");
        }

        field.FieldName = request.FieldName;
        field.DataType = request.DataType;
        field.IsRelevant = request.IsRelevant;
        field.DisplayOrder = request.DisplayOrder;
        repository.Update(field);
        await repository.SaveChangesAsync(cancellationToken);
        return (Map(field), null);
    }

    private static MetricFieldResponse Map(MetricField field) => new(
        field.Id,
        field.MetricId,
        field.FieldName,
        field.DataType,
        field.IsRelevant,
        field.DisplayOrder);
}
