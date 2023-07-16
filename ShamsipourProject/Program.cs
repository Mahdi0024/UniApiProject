using UniApiProject.Data;
using UniApiProject.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using UniApiProject.Services;
using UniApiProject.Models;
using UniApiProject.Middlewares;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
services.AddSwaggerGen();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });
services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", p =>
    {
        p.RequireClaim("admin", "true");
    });
    options.AddPolicy("Student", p =>
    {
        p.RequireClaim("student");
    });
    options.AddPolicy("Teacher", p =>
    {
        p.RequireClaim("teacher");
    });
});
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddDbContext<ApiDbContext>(o => o.UseSqlServer(config.GetConnectionString("SqlServer")));

services.AddTransient<CourseService>();
services.AddTransient<UserService>();
services.AddSingleton<AuthService>();
services.AddSingleton<FileServiceConfiguration>(
    new FileServiceConfiguration
    {
        FileDirectory = config["FileSettings:FileDirectory"]!,
        TempDirectory = config["FileSettings:TempDirectory"]!
    });
services.AddTransient<FileService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();