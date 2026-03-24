using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false);

builder.Services.AddOcelot(builder.Configuration);



builder.Services.AddMemoryCache();



builder.Services.AddHttpClient("ProgressAPI", client =>
{
    client.BaseAddress = new Uri("https://progres.mesrs.dz/api/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5174","http://localhost:5173", "http://localhost:3005", "http://localhost:3006")
                  .AllowCredentials()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();


app.MapGet("/", () => "Hello World!");

app.Run();
