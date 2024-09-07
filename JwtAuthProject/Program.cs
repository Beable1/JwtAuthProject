using JwtAuthProject.Core.Configuration;
using JwtAuthProject.Core.Repositories;
using JwtAuthProject.Core.Services;
using JwtAuthProject.Service.Services;
using JwtAuthProject.Core.UnitOfWork;
using JwtAuthProject.Data;
using JwtAuthProject.Data.Repositories;
using JwtAuthProject.Service.Mappers;

using SharedLibrary.Configuration;
using SharedLibrary.Extensions;

using Microsoft.AspNetCore.Authentication;
using SharedLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using JwtAuthProject.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Sevices;
using FluentValidation.AspNetCore;
using JwtAuthProject.Service.Validations;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.UseCustomValidationResponse();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

builder.Services.AddScoped<JwtAuthProject.Core.Services.IAuthenticationService, JwtAuthProject.Service.Services.AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddDbContext<Context>(
    x =>
    {
        x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
        {
            option.MigrationsAssembly(Assembly.GetAssembly(typeof(Context)).GetName().Name);
        });
    }
    );


builder.Services.AddIdentity<UserApp, IdentityRole>(Opt =>
{
    Opt.User.RequireUniqueEmail = true;
    Opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();


builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOption"));

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptions>();
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero



    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
