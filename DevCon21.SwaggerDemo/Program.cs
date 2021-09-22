using System.Text;
using DevCon21.SwaggerDemo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoListContext>(
    opt => opt.UseInMemoryDatabase("TodoList"))
    .AddControllers();

var secret = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:JwtToken:Secret"));
builder.Services.AddAuthentication(configure =>
{
    configure.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configure.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(configure =>
{
    configure.SaveToken = true;
    configure.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers().RequireAuthorization());
app.MapControllers();

// Initialize In-Memory database
using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<TodoListContext>();
context.Database.EnsureCreated();

app.Run();
