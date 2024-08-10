using Microsoft.OpenApi.Models;
using WineCollectionManagerApi.Initialization;
using WineCollectionManagerApi.Mappings;
using WineCollectionManagerApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddSingleton<IWinemakerService, WinemakerService>();
builder.Services.AddSingleton<IWineBottleService, WineBottleService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Wine Collection Manager API",
        Description = "A simple example ASP.NET Core Web API for managing a wine collection",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wine Collection Manager API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var winemakerService = scope.ServiceProvider.GetRequiredService<IWinemakerService>();
    var wineBottleService = scope.ServiceProvider.GetRequiredService<IWineBottleService>();
    await SampleDataInitializer.Initialize(winemakerService, wineBottleService);
}

app.Run();
