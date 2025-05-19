using CurrencyExchangeApi;
using CurrencyExchangeApi.Dtos.Requests;
using CurrencyExchangeApi.Proxies;
using CurrencyExchangeApi.Services;
using CurrencyExchangeApi.Validators;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddMemoryCache(); // <-- Added memory cache registration
builder.Services.AddHttpClient<IFrankfurterApiProxy, FrankfurterApiProxy>();

// Register FluentValidation validator
builder.Services.AddScoped<IValidator<CreateQuoteRequest>, CreateQuoteRequestValidator>();
builder.Services.AddScoped<IValidator<TransferRequest>, TransferRequestValidator>();

// Logging configuration (default includes Console, Debug, EventSource)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
