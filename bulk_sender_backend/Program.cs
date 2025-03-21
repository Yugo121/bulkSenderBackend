using Application.Models.Commands.BrandCommands;
using Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

new InfrastructureService(builder.Services, builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddBrandCommand).Assembly));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy.WithOrigins("http://localhost:5173/", "http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader());
});


var app = builder.Build();

app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
