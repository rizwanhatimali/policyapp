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
tempLogger.LogInformation("Connection string from environment: {ConnectionString}", connectionString);

builder.Services.AddDbContext<PolicyDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

tempLogger.LogInformation("Swagger endpoint exposed for all env");

var app = builder.Build();

app.MapGet("/", () => "Hello from Policy API!");
app.MapGet("/info", () => "Info from Policy API!");

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
