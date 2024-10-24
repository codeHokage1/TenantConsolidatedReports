using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports;
using TenantConsolidatedReports.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register background service
builder.Services.AddHostedService<BusinessReportUpdateService>();

// Add DbContext for report
builder.Services.AddDbContext<ReportDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConsolidatedReport"));
});

// Add DbContext for identity dbs
builder.Services.AddDbContext<IdentityDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Organization"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
