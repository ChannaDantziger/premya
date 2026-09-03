using Microsoft.EntityFrameworkCore;
using Premya.Api.Domain.Entities;

namespace Premya.Api.Infrastructure.Persistence;

public class PremyaDbContext(DbContextOptions<PremyaDbContext> options) : DbContext(options)
{
    public DbSet<PremiumMethod> PremiumMethods => Set<PremiumMethod>();
    public DbSet<Metric> Metrics => Set<Metric>();
    public DbSet<MetricField> MetricFields => Set<MetricField>();
    public DbSet<FileStructureVersion> FileStructureVersions => Set<FileStructureVersion>();
    public DbSet<FileStructureField> FileStructureFields => Set<FileStructureField>();
    public DbSet<ImportBatch> ImportBatches => Set<ImportBatch>();
    public DbSet<DynamicRecord> DynamicRecords => Set<DynamicRecord>();
    public DbSet<DynamicValue> DynamicValues => Set<DynamicValue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PremiumMethod>(entity =>
        {
            entity.HasIndex(item => item.MethodNumber).IsUnique();
            entity.Property(item => item.PremiumRate).HasPrecision(5, 2);
        });

        modelBuilder.Entity<Metric>()
            .HasIndex(item => new { item.PremiumMethodId, item.Name })
            .IsUnique();

        modelBuilder.Entity<MetricField>()
            .HasIndex(item => new { item.MetricId, item.FieldName })
            .IsUnique();

        modelBuilder.Entity<FileStructureVersion>()
            .HasIndex(item => new { item.MetricId, item.VersionNumber })
            .IsUnique();

        modelBuilder.Entity<FileStructureField>()
            .HasIndex(item => new { item.FileStructureVersionId, item.FieldName })
            .IsUnique();

        modelBuilder.Entity<DynamicRecord>()
            .HasIndex(item => new { item.ImportBatchId, item.RowNumber })
            .IsUnique();

        modelBuilder.Entity<DynamicValue>()
            .HasIndex(item => new { item.DynamicRecordId, item.FileStructureFieldId })
            .IsUnique();

        modelBuilder.Entity<Metric>()
            .HasOne(item => item.PremiumMethod)
            .WithMany(item => item.Metrics)
            .HasForeignKey(item => item.PremiumMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FileStructureField>()
            .HasOne(item => item.MetricField)
            .WithMany(item => item.FileStructureFields)
            .HasForeignKey(item => item.MetricFieldId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
