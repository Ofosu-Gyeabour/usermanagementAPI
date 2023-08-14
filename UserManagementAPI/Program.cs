global using UserManagementAPI.Data;
global using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources.Implementations;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.utils;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = @"UserManagementAPI",
        Description = @"API handling all user authentication and management",
        Version = "v1"
    });
});


#region custom-repository

builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
builder.Services.AddSingleton<IUserService, UserService>();

#endregion

#region CORS

builder.Services.AddCors(options => options.AddPolicy(@"ApiCorsPolicy",builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

#endregion

var app = builder.Build();

#region app cors

app.UseCors(@"ApiCorsPolicy");

#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagementAPI v1.0");
    });
}

//app.UseHttpsRedirection();

#region settings-configuration

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
ConfigObject.SCAFFOLD = settings.scaffold;
ConfigObject.DB_CONN = settings.dbConnectionString;

#endregion


#region api - resources

app.MapGet("/Department/GetDepartments", async (IDepartmentService service) => await GetDepartments(service));

app.MapGet("/User/GetUsers", async (IUserService usrservice) => await GetUsers(usrservice));

app.MapPost("/User/GetMD5Encryption", async (SingleParam userData, IUserService usrservice) => await GetMD5Encryption(userData, usrservice))
            .Produces<DefaultAPIResponse>(StatusCodes.Status200OK)
            .WithName("Authenticate Password using MD5 Encryption")
            .WithTags("Authentication");

app.MapPost("/User/AuthenticateUser", async (UserInfo userCredential, IUserService usrservice) => await AuthenticateUser(userCredential, usrservice)).WithTags("Authentication");

#endregion

#region api - tasks

async Task<IResult> GetDepartments(IDepartmentService service)
{
    var dta = await service.GetDepartmentsAsync();
    return Results.Ok(dta);
}

async Task<IResult> GetUsers(IUserService usrservice)
{
    var usrs = await usrservice.GetUsersAsync();
    return Results.Ok(usrs);
}

async Task<IResult> GetMD5Encryption(SingleParam userData, IUserService usrservice)
{
    var returned = await usrservice.GetMD5EncryptedPasswordAsync(userData);
    return Results.Ok(returned);
}

async Task<IResult> AuthenticateUser(UserInfo userCredential, IUserService usrservice)
{
    if (userCredential != null)
    {
        var results = await usrservice.GetUserAsync(userCredential);
        return Results.Ok(results);
    }
    else
    {
        return Results.BadRequest();
    }
}

#endregion

app.Run();