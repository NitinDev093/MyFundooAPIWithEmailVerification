using BusinessLayer.BusinessLayer;
using BusinessLayer.IBusinesslayer;
using Microsoft.OpenApi.Models;
using RepositoryLayer.IRepositoryLayer;
using RepositoryLayer.RepositoryLayer;
using UtilityLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//My Activity
var jwtSection = builder.Configuration.GetSection("JwtSettings");
string secretKey = jwtSection["SecretKey"];
//End

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//My Activatity Start=========================
builder.Services.AddScoped<IUserRepositoryLayer, UserRepositoryLayer>();
builder.Services.AddScoped<IUserBusinessLayer, UserBusinessLayer>();
builder.Services.AddScoped<INotesRepositoryLayer, NotesRepositoryLayer>();
builder.Services.AddScoped<INotesBusinsessLayer, NotesBusinessLayer>();
builder.Services.AddSingleton<EmailHelper>();
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddCors(options =>//Enabling Cors
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200")//This is angular frontent URL
            .AllowAnyMethod()   
            .AllowAnyHeader(); 
    });
});
// 🔹 Swagger with JWT Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Fundoo API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token below."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});
// 🔹 JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
//My Activatity End===========================


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//My Activity
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseMiddleware<UtilityLayer.JwtMiddleware>();  // always use within UseAuthentication & UseAuthorization
//end
app.UseAuthorization();

app.MapControllers();


app.Run();
