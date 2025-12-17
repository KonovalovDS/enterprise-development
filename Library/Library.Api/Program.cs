using Library.Application.Contracts.Mappers;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
    );
});

builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "DefaultConnection");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var secretKeyBase64 = Environment.GetEnvironmentVariable("JwtSettingsSecretKey")
                      ?? throw new InvalidOperationException("JWT SecretKey not found");
var secretKeyBytes = Convert.FromBase64String(secretKeyBase64);

var issuer = Environment.GetEnvironmentVariable("JwtSettingsIssuer")!;
var audience = Environment.GetEnvironmentVariable("JwtSettingsAudience")!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();

builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    var contractsXml = Path.Combine(AppContext.BaseDirectory, "Library.Application.Contracts.xml");
    if (File.Exists(contractsXml))
        c.IncludeXmlComments(contractsXml);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "¬ведите JWT токен"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    db.Database.Migrate();

    await DbSeeder.SeedBooksAsync(db);
    await DbSeeder.SeedCustomersAsync(db);
    await DbSeeder.SeedBorrowRecordsAsync(db);

    await DbSeeder.SeedUsersAndRolesAsync(db, userManager, roleManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();

app.Run();