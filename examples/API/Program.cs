using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tekoding.KoIdentity.Core;
using Tekoding.KoIdentity.Core.Stores;
using Tekoding.KoIdentity.Core.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<UserValidator>();
builder.Services.AddTransient<IUserStore, UserStore>();
builder.Services.AddDbContext<DbContext, DatabaseContext>(optionsAction => optionsAction.UseSqlServer(
    Environment.GetEnvironmentVariable("TekodingAzureDEVConnection") ??
    throw new InvalidOperationException()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "KoIdentity API",
        Description = "An ASP.NET Core Web API for managing users.",
        TermsOfService = new Uri($"{builder.Configuration.GetSection("ApplicationSettings:URL").Value}/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri($"{builder.Configuration.GetSection("ApplicationSettings:URL").Value}/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri($"{builder.Configuration.GetSection("ApplicationSettings:URL").Value}/license")
        }
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header

            },
            new List<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();