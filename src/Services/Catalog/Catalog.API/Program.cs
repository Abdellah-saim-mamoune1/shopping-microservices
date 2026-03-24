using Catalog.API.GrpcServices;
using Catalog.API.Repositories;
using Catalog.API.Seed;
using Catalog.API.Services;
using Catalog.Application.Services;
using Catalog.Infrastructure.Repositories;
using Catalog.Settings;
using Discount.GRPC.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Services
builder.Services.AddScoped<Seeder>();

builder.Services.AddScoped<IBookCustomerService, BookCustomerService>();
builder.Services.AddScoped<IBookAdminService, BookAdminService>();


//Repos
builder.Services.AddScoped<IBookRepository, BookRepository>(); 


builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
               (o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!));

builder.Services.AddScoped<DiscountGrpcService>();
// Register MongoClient
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});




builder.Services.AddScoped<BookRepository>();






builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),

        };



    });


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var Seeder = scope.ServiceProvider.GetRequiredService<Seeder>();

    await Seeder.SeedAsync();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<UnifiedResponseMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
