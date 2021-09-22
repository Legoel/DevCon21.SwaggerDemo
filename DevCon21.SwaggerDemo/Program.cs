using System.Reflection;
using System.Text;
using DevCon21.SwaggerDemo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var documentName = "tlm_v1";

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.AddSwaggerGen(configure =>
{
    configure.SwaggerDoc(documentName, new OpenApiInfo
    {
        Title = "Another Todo List Management Tool",
        Version = "v1"
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT authentication",
        Description = "Enter JWT Bearer token only",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    configure.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    configure.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
    configure.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    configure.ExampleFilters();
    configure.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
});

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

app.UseSwagger(configure =>
{
    configure.RouteTemplate = "documentation/{documentName}/tlm.json";
});
app.UseSwaggerUI(configure =>
{
    configure.RoutePrefix = "documentation";
    configure.SwaggerEndpoint($"{documentName}/tlm.json", "TLM V1");
});
app.UseReDoc(configure =>
{
    configure.RoutePrefix = "redoc";
    configure.SpecUrl = $"/documentation/{documentName}/tlm.json";
    configure.DocumentTitle = "TLM Documentation";
});

// Initialize In-Memory database
using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<TodoListContext>();
context.Database.EnsureCreated();

app.Run();
