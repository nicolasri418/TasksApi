// Program.cs
using System.Text.Json.Serialization;
using TasksApi.Services;

var builder = WebApplication.CreateBuilder(args);




const string CorsPolicy = "AllowAngular";
builder.Services.AddCors(o => o.AddPolicy(CorsPolicy, p =>
    p.WithOrigins("http://localhost:4200")
     .AllowAnyHeader()
     .AllowAnyMethod()));

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaskStore, InMemoryTaskStore>();

var app = builder.Build();

// Solo forzar HTTPS fuera de Development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsPolicy);

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();