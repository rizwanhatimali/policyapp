using Microsoft.EntityFrameworkCore;
using Service.PolicyDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["CONNECTION_STRING"];

var tempLoggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
});
var tempLogger = tempLoggerFactory.CreateLogger("Startup");
tempLogger.LogInformation("Connection string recieved from environment: {ConnectionString}", connectionString);

tempLogger.LogInformation("Temp Removed all db connectivity");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

tempLogger.LogInformation("Swagger endpoint exposed for all env");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PolicyDbContext>();
    db.Database.Migrate();
}

tempLogger.LogInformation("default and info endpoint exposed");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
