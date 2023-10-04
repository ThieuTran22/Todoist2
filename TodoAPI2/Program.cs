using Microsoft.EntityFrameworkCore;
using TodoAPI2;
using TodoAPI2.Models;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddPolicy(name: MyAllowSpecificOrigins, policy => policy.WithOrigins("*").AllowAnyMethod().AllowAnyMethod()));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

var app = builder.Build();
app.MapGet("/", () => "Đây là đường dẫn mặc định, chức mừng đã chạy thành công!");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(MyAllowSpecificOrigins);

app.MapmProjectEndpoints();

app.MapmTaskEndpoints();

app.Run();

