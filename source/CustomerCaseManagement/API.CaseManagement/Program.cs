using Infra.CaseManagement;
using Microsoft.EntityFrameworkCore;
using Application.CaseManagement.Interface;
using Application.CaseManagement.Service;
using Infra.CaseManagement.Repositories;
using FluentValidation;
using Application.CaseManagement.Validator;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<SupportCaseValidator>();
// Learn more about configuring Swagger at https://aka.ms/aspnet/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CaseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ISupportCaseService, SupportCaseService>();
builder.Services.AddScoped<ISupportCaseRepository, SupportCaseRepository>();


var app = builder.Build();
app.UseExceptionHandler();

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
