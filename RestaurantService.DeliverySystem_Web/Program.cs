
using AutoMapper;
using DeliverySystem_MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantService.DeliverySystem_DAL.Abstract.Repositories;
using RestaurantService.DeliverySystem_DAL.Abstract.Services;
using RestaurantService.DeliverySystem_DAL.Context;
using RestaurantService.DeliverySystem_DAL.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RestaurantDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Restaurant")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("Cors", builder =>
	{
		builder.AllowAnyHeader()
		.AllowAnyMethod()
		.WithOrigins("http://localhost:4200")
		.AllowCredentials();
	});
});

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, RestaurantService.DeliverySystem_BAL.Services.ProductService>();


builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, RestaurantService.DeliverySystem_BAL.Services.OrderService>();


var mapperConfig = new MapperConfiguration(mc =>
{
	mc.AddProfile(new MappingProfile());
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters //Podesavamo parametre za validaciju pristiglih tokena
        {
            ValidateIssuer = true, //Validira izdavaoca tokena
            ValidateAudience = false, //Kazemo da ne validira primaoce tokena
            ValidateLifetime = true,//Validira trajanje tokena
            ValidateIssuerSigningKey = true, //validira potpis token, ovo je jako vazno!
            ValidIssuer = "http://localhost:7007", //odredjujemo koji server je validni izdavalac
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key used for authentication"))//navodimo privatni kljuc kojim su potpisani nasi tokeni
        };
    });

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Cors");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();