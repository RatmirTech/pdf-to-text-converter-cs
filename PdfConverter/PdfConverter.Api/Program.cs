using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PdfConverter", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

string[] origins = { "https://localhost:7255" };
builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
{
    builder.WithOrigins().AllowAnyMethod()
    .AllowAnyHeader().AllowCredentials();
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()!
        .Error;
    var response = new { exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors();
app.Run();