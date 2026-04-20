using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // ✅ NEW (instead of SwaggerGen)

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("LibraryDb"));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // ✅ replaces UseSwagger()

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Library API");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

// Redirect root → Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();