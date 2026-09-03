using Microsoft.EntityFrameworkCore;
using System.Text;
using Premya.Api.Application.Interfaces;
using Premya.Api.Application.Services;
using Premya.Api.Infrastructure.Persistence;
using Premya.Api.Infrastructure.Repositories;
using Premya.Api.Infrastructure.Excel;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options => options.AddPolicy("Client", policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddDbContext<PremyaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PremyaDatabase")));
builder.Services.AddScoped<IPremiumMethodRepository, PremiumMethodRepository>();
builder.Services.AddScoped<IPremiumMethodService, PremiumMethodService>();
builder.Services.AddScoped<IMetricRepository, MetricRepository>();
builder.Services.AddScoped<IMetricService, MetricService>();
builder.Services.AddScoped<IMetricFieldRepository, MetricFieldRepository>();
builder.Services.AddScoped<IMetricFieldService, MetricFieldService>();
builder.Services.AddScoped<IExcelReader, ExcelReader>();
builder.Services.AddScoped<IImportRepository, ImportRepository>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IDynamicDataRepository, DynamicDataRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<PremyaDbContext>();
    database.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Client");
app.MapControllers();

app.Run();
