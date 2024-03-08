using API.Error;
using API.Extensions;
using API.Middleware;
using API.Services;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddApplicationServices();
builder.Services.AddIdentityServicesServices(builder.Configuration);
builder.Services.AddAuthorizationPolicyServices();
builder.Services.AddCors(opt => {
    opt.AddPolicy("CorsPolicy", policy => {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["JWT:ClientUrl"]);
    });
});
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.InvalidModelStateResponseFactory = actionContext => {
        var erorrs = actionContext.ModelState
      .Where(e => e.Value.Errors.Count > 0)
       .SelectMany(x => x.Value.Errors)
        .Select(x => x.ErrorMessage).ToArray();
        var erroResponse = new FailResponse(400)
        {

            Errors = erorrs

        };
        return new BadRequestObjectResult(erroResponse);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddswaggerDocmentation();
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("CorsPolicy");
 
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocmentation();
     
}
app.UseStatusCodePagesWithReExecute("/erorr/{0}");
app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
