using Microsoft.EntityFrameworkCore;
using TinyMartAPI.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<TinyMartDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();   // <-- enables [ApiController]
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 
  

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
     app.UseSwagger();                
    app.UseSwaggerUI();       
   
}


// app.UseHttpsRedirection();

app.UseAuthorization();

// Map controllers like AudioController
app.MapControllers();

app.Run();
