var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS (must be before other middleware)
app.UseCors("AllowAll");

// Optional: disable HTTPS redirect for testing
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Backend starting...");
app.Run();
Console.WriteLine("Backend stopped.");