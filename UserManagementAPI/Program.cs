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

//added
//builder.Services.AddResponseCaching(x => x.MaximumBodySize = 1024);

#region custom-repository

builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IProfileService, ProfileService>();


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

#region added

//app.UseResponseCaching();

//app.Use(async (context, next) =>
//{
//    context.Response.GetTypedHeaders().CacheControl =
//        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
//        {
//            Public = true,
//            MaxAge = TimeSpan.FromSeconds(10)
//        };

//    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };
//    await next();
//});

#endregion

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagementAPI v1.0");
    });
//}

//app.UseHttpsRedirection();

#region settings-configuration

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
ConfigObject.SCAFFOLD = settings.scaffold;
ConfigObject.DB_CONN = settings.dbConnectionString;
ConfigObject.LOCAL_CONN = settings.localConnString;
ConfigObject.TEST_CONN = settings.testConn;

#endregion


#region api - resources

app.MapGet("/Department/GetDepartments", async (IDepartmentService service) => await GetDepartmentsAsync(service)).WithTags("Department");

app.MapGet("/User/GetUsers", async (IUserService usrservice) => await GetUsersAsync(usrservice));

app.MapPost("/User/GetMD5Encryption", async (SingleParam userData, IUserService usrservice) => await GetMD5Encryption(userData, usrservice)).WithTags("Authentication");

app.MapPost("/User/AuthenticateUser", async (UserInfo userCredential, IUserService usrservice) => await AuthenticateUserAsync(userCredential, usrservice)).WithTags("Authentication");
app.MapPost("/User/SetLoginFlag", async(UserInfo user, IUserService service)=> await SetLoginFlagAsync(user,service)).WithTags("Authentication");
app.MapPost("/User/SetLogoutFlag", async (UserInfo user, IUserService service) => await SetLogoutFlagAsync(user, service)).WithTags("Authentication");
app.MapPost("/User/GetUserProfile", async (UserInfo _user, IUserService service) => await GetUserProfileAsync(_user, service)).WithTags("Authentication");
app.MapPost("/User/AmendUserProfile", async (UserProfile userProfile, IUserService service)=> await AmendUserProfileAsync(userProfile, service)).WithTags("Authentication");
app.MapPost("/User/CreateAccount", async(userRecord _userRecord, IUserService service)=>await CreateUserAccountAsync(_userRecord,service)).WithTags("Authentication");
app.MapPost("/Log/WriteLog",async(Log oLogger, ILoggerService service)=> await WriteLogAsync(oLogger,service)).WithTags("Logger");
app.MapGet("/Log/GetLogs",async(ILoggerService service)=> await GetLogDataAsync(service)).WithTags("Logger");
app.MapPost("/Log/GetUserLogs",async(SingleParam _param, ILoggerService service)=> await GetUserLogsAsync(_param, service)).WithTags("Logger");
app.MapPost("/Profile/CreateProfile", async(SystemProfile oProfile, IProfileService service) => await CreateProfileAsync(oProfile,service)).WithTags("Profile");
app.MapPost("/Profile/AmendProfile", async (SystemProfile oProfile, IProfileService service) => await AmendProfileAsync(oProfile, service)).WithTags("Profile");
app.MapPost("/Profile/GetProfiles", async(SingleParam companyIdentifier, IProfileService service) => await GetProfileListAsync(companyIdentifier, service)).WithTags("Profile");
app.MapPost("/Profile/GetProfileModules", async(SingleParam oProfileObj, IProfileService  service) => await GetProfileModulesAsync(oProfileObj, service)).WithTags("Profile");

#endregion

#region api - tasks

async Task<IResult> GetDepartmentsAsync(IDepartmentService service)
{
    try
    {
        var dta = await service.GetDepartmentsAsync();
        return Results.Ok(dta);
    }
    catch(Exception x)
    {
        return Results.BadRequest($"Error: {x.Message}");
    }
}

async Task<IResult> GetUsersAsync(IUserService usrservice)
{
    try
    {
        var usrs = await usrservice.GetUsersAsync();
        return Results.Ok(usrs);
    }
    catch (Exception e)
    {
        return Results.BadRequest($"Error: {e.Message}");
    } 
}

async Task<IResult> GetMD5Encryption(SingleParam userData, IUserService usrservice)
{
    try
    {
        var returned = await usrservice.GetMD5EncryptedPasswordAsync(userData);
        return Results.Ok(returned);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

async Task<IResult> AuthenticateUserAsync(UserInfo userCredential, IUserService usrservice)
{
    try
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
    catch(Exception e)
    {
        return Results.BadRequest(e.Message);
    }  
}

async Task<IResult> WriteLogAsync(Log oLogger, ILoggerService service)
{
    try
    {
        if (oLogger == null)
        {
            return Results.BadRequest();
        }
        else
        {
            var logOperation = await service.WriteLogAsync(oLogger);
            return Results.Ok(logOperation);
        }
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    } 
}

async Task<IResult> GetLogDataAsync(ILoggerService service)
{
    try
    {
        if (service == null)
        {
            return Results.BadRequest();
        }
        else
        {
            var _LogData = await service.GetLogsAsync();
            return Results.Ok(_LogData);
        }
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex);
    }
    
}

async Task<IResult> GetUserLogsAsync(SingleParam _param, ILoggerService service)
{
    try
    {
        if (_param.stringValue.Length == 0)
        {
            return Results.BadRequest();
        }
        else
        {
            var user_log_data = await service.GetLogsAsync(_param);
            return Results.Ok(user_log_data);
        }
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex);
    }
}

async Task<IResult> SetLoginFlagAsync(UserInfo user, IUserService service)
{
    try
    {
        if (user == null)
        {
            return Results.BadRequest();
        }

        var statusObj = await service.SetLoggedFlagAsync(user);
        return Results.Ok(statusObj);
    }
    catch (Exception x)
    {
        return Results.BadRequest($"error: {x.Message}");
    }
    
}

async Task<IResult> SetLogoutFlagAsync(UserInfo user, IUserService service)
{
    try
    {
        if (user == null)
        {
            return Results.BadRequest();
        }

        var statusObj = await service.SetLoggedOutFlagAsync(user);
        return Results.Ok(statusObj);
    }
    catch(Exception x)
    {
        return Results.BadRequest($"error: {x.Message}");
    }
}

async Task<IResult> CreateProfileAsync(SystemProfile oProfile, IProfileService service)
{
    try
    {
        if (oProfile.nameOfProfile == string.Empty)
        {
            return Results.BadRequest();
        }

        var profileStatus = await service.SaveProfileAsync(oProfile);
        return Results.Ok(profileStatus);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

async Task<IResult> AmendProfileAsync(SystemProfile oProfile,IProfileService service)
{
    try
    {
        if (oProfile.nameOfProfile.Length == 0)
        {
            return Results.BadRequest();
        }

        var status = await service.AmendProfileAsync(oProfile);
        return Results.Ok(status);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }   
}

async Task<IResult> GetProfileListAsync(SingleParam companyIdentifier, IProfileService service)
{
    //gets the list of profiles in the data store
    try
    {
        if (companyIdentifier == null)
        {
            return Results.BadRequest();
        }

        var profile_list = await service.GetProfilesAsync(int.Parse(companyIdentifier.stringValue));
        return Results.Ok(profile_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x.Message);
    }
    
}

async Task<IResult> GetUserProfileAsync(UserInfo _user,IUserService service)
{
    try
    {
        if (_user.username.Length == 0)
        {
            return Results.BadRequest();
        }

        var responseData = await service.GetUserProfileAsync(_user);
        return Results.Ok(responseData);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

async Task<IResult> GetProfileModulesAsync(SingleParam oProfileObj, IProfileService service)
{
    try
    {
        if (oProfileObj.stringValue.Length == 0)
        {
            return Results.BadRequest();
        }

        var obj = new UserInfo() { };
        var profile = await service.GetProfileModulesAsync(oProfileObj);
        return Results.Ok(profile);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

#region Authentication

async Task<IResult> AmendUserProfileAsync(UserProfile userProfile, IUserService service)
{
    try
    {
        if (userProfile.username.Length == 0)
        {
            return Results.BadRequest(@"username is empty");
        }

        if (userProfile.profile.nameOfProfile.Length == 0)
        {
            return Results.BadRequest(@"profile name is empty");
        }

        var amendStatus = await service.AmendUserProfileAsync(userProfile);
        return Results.Ok(amendStatus);
    }
    catch(Exception err)
    {
        return Results.BadRequest($"error: {err.Message}");
    }
}

async Task<IResult> CreateUserAccountAsync(userRecord _userRecord, IUserService service)
{
    try
    {
        var user_creation_results = await service.CreateUserAsync(_userRecord);
        return Results.Ok(user_creation_results);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

#endregion

#endregion

app.Run();