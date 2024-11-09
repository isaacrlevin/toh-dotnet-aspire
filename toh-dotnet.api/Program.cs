using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using tohdotnet.domain;
using tohdotnet.domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddTransient<IHeroService, HeroService>();

//builder.Services.AddDbContext<tohdotnetContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("tohdotnetContext")));

builder.AddSqlServerDbContext<tohdotnetContext>("heroes");

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapGet("/Heroes", async (IHeroService service, string? name) =>
{
    if (!string.IsNullOrEmpty(name))
    {
        return await service.SearchHeros(name);
    }
    else
    {
        return await service.GetHeros();
    }
});

app.MapGet("/Heroes/{id}", async (IHeroService service, int id) =>
{
    return await service.GetHero(id);
});

app.MapPut("/Heroes/{id}", async (IHeroService service, int id, Hero hero) =>
{
    try
    {
        await service.UpdateHero(id, hero);
    }
    catch (DbUpdateConcurrencyException)
    {
        var tempHero = await service.GetHero(id);
        if (tempHero != null)
        {
            throw;
        }
    }
});

app.MapPost("/Heroes", async (IHeroService service, Hero hero) =>
{
    return await service.CreateHero(hero);
});

app.MapDelete("/Heroes/{id}", async (IHeroService service, int id) =>
{
    var hero = await service.GetHero(id);
    if (hero != null)
    {
        await service.DeleteHero(hero);
    }
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
