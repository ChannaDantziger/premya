using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Premya.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PremiumMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MethodNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PremiumRate = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                    CalculationPeriod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PremiumMethodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SourceType = table.Column<string>(type: "TEXT", nullable: false),
                    SourceName = table.Column<string>(type: "TEXT", nullable: false),
                    IngestionFrequency = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metrics_PremiumMethods_PremiumMethodId",
                        column: x => x.PremiumMethodId,
                        principalTable: "PremiumMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileStructureVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    VersionNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStructureVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileStructureVersions_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetricFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldName = table.Column<string>(type: "TEXT", nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    IsRelevant = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetricFields_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MetricId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileStructureVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    DataYear = table.Column<int>(type: "INTEGER", nullable: false),
                    CalculationPeriod = table.Column<string>(type: "TEXT", nullable: false),
                    ImportedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true),
                    RecordCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportBatches_FileStructureVersions_FileStructureVersionId",
                        column: x => x.FileStructureVersionId,
                        principalTable: "FileStructureVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportBatches_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileStructureFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileStructureVersionId = table.Column<int>(type: "INTEGER", nullable: false),
                    MetricFieldId = table.Column<int>(type: "INTEGER", nullable: true),
                    FieldName = table.Column<string>(type: "TEXT", nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    IsRelevant = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStructureFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileStructureFields_FileStructureVersions_FileStructureVersionId",
                        column: x => x.FileStructureVersionId,
                        principalTable: "FileStructureVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileStructureFields_MetricFields_MetricFieldId",
                        column: x => x.MetricFieldId,
                        principalTable: "MetricFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DynamicRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImportBatchId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicRecords_ImportBatches_ImportBatchId",
                        column: x => x.ImportBatchId,
                        principalTable: "ImportBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DynamicRecordId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileStructureFieldId = table.Column<int>(type: "INTEGER", nullable: false),
                    ValueText = table.Column<string>(type: "TEXT", nullable: true),
                    ValueNumber = table.Column<decimal>(type: "TEXT", nullable: true),
                    ValueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValueBoolean = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicValues_DynamicRecords_DynamicRecordId",
                        column: x => x.DynamicRecordId,
                        principalTable: "DynamicRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicValues_FileStructureFields_FileStructureFieldId",
                        column: x => x.FileStructureFieldId,
                        principalTable: "FileStructureFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicRecords_ImportBatchId_RowNumber",
                table: "DynamicRecords",
                columns: new[] { "ImportBatchId", "RowNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicValues_DynamicRecordId_FileStructureFieldId",
                table: "DynamicValues",
                columns: new[] { "DynamicRecordId", "FileStructureFieldId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicValues_FileStructureFieldId",
                table: "DynamicValues",
                column: "FileStructureFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FileStructureFields_FileStructureVersionId_FieldName",
                table: "FileStructureFields",
                columns: new[] { "FileStructureVersionId", "FieldName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStructureFields_MetricFieldId",
                table: "FileStructureFields",
                column: "MetricFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FileStructureVersions_MetricId_VersionNumber",
                table: "FileStructureVersions",
                columns: new[] { "MetricId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatches_FileStructureVersionId",
                table: "ImportBatches",
                column: "FileStructureVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatches_MetricId",
                table: "ImportBatches",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricFields_MetricId_FieldName",
                table: "MetricFields",
                columns: new[] { "MetricId", "FieldName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_PremiumMethodId_Name",
                table: "Metrics",
                columns: new[] { "PremiumMethodId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PremiumMethods_MethodNumber",
                table: "PremiumMethods",
                column: "MethodNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicValues");

            migrationBuilder.DropTable(
                name: "DynamicRecords");

            migrationBuilder.DropTable(
                name: "FileStructureFields");

            migrationBuilder.DropTable(
                name: "ImportBatches");

            migrationBuilder.DropTable(
                name: "MetricFields");

            migrationBuilder.DropTable(
                name: "FileStructureVersions");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "PremiumMethods");
        }
    }
}
