using BusinessLayer.BusinessLayer;
using BusinessLayer.IBusinesslayer;
using RepositoryLayer.IRepositoryLayer;
using RepositoryLayer.RepositoryLayer;
using UtilityLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//My Activatity Start=========================
builder.Services.AddScoped<IUserRepositoryLayer, UserRepositoryLayer>();
builder.Services.AddScoped<IUserBusinessLayer, UserBusinessLayer>();
builder.Services.AddSingleton<EmailHelper>();
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200")   // ya specific origin: .WithOrigins("https://example.com")
            .AllowAnyMethod()   // GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader();  // allow headers
    });
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
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
