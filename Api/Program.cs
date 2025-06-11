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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "Hello from Policy API!");
app.MapGet("/info", () => "Info from Policy API!");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
