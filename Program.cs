using Microsoft.EntityFrameworkCore;
using WellsAPI.Data;
using WellsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
/*builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));*/

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString != null && connectionString.StartsWith("postgresql://"))
{
    var uri = new Uri(connectionString);
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<WellImportService>();
builder.Services.AddScoped<WellService>();

//Setagem para CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                    "http://localhost:5173",
                    "http://localhost:3002",
                    "http://localhost:5000",
                    "https://wells-frontend-one.vercel.app"
                )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Wells API v1");
    });
}

//Rodar Migration para Deploy
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();