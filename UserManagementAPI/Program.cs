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
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IProfileService, ProfileService>();
builder.Services.AddSingleton<ICityService, CityService>();
builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<IReferralService, ReferralService>();
builder.Services.AddSingleton<ICompanyService, CompanyService>();
builder.Services.AddSingleton<IModuleService, ModuleService>();
builder.Services.AddSingleton<IRegionService, RegionService>();

builder.Services.AddSingleton<IBranchService, BranchService>();
builder.Services.AddSingleton<ITitleService, TitleService>();
builder.Services.AddSingleton<IShippingPortService, ShippingPortService>();

builder.Services.AddSingleton<IAirportService, AirportService>();
builder.Services.AddSingleton<IDialCodeService, DialCodeService>();
builder.Services.AddSingleton<IContainerTypeService, ContainerTypeService>();

builder.Services.AddSingleton<IAdhocTypeService, AdhocTypeService>();
builder.Services.AddSingleton<IPackagingService, PackagingService>();
builder.Services.AddSingleton<ISealService, SealService>();

builder.Services.AddSingleton<IShippingPortService, ShippingPortService>();
builder.Services.AddSingleton<IShippingService, ShippingService>();

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
ConfigObject.MAC_LOCAL_CONN = settings.macConnString;
ConfigObject.TEST_CONN = settings.testConn;


var eventSettings = builder.Configuration.GetSection("Events").Get<Events>();
EventConfig.AUTH_OPERATION = eventSettings.auth;
EventConfig.EXIT_OPERATION = eventSettings.exit;
EventConfig.ADD_RECORD_OPERATION = eventSettings.recordAdd;

#endregion


#region api - resources

#region shipping-line routes

app.MapGet("/ShippingLine/List", async (IShippingService service) => await GetShippingLineListAsync(service)).WithTags("Shipping Line");
app.MapPost("/ShippingLine/Create", async (ShippingLineLookup oShippingLine, IShippingService service) => await CreateShippingLineAsync(oShippingLine, service)).WithTags("Shipping Line");
#endregion

#region shipping vessel - routes
app.MapGet("/ShippingVessel/List",  (IShippingService service) => GetShippingVesselListAsync(service)).WithTags("Shipping Vessel");
app.MapPost("/ShippingVessel/Create", async (VesselLookup oVessel, IShippingService service) => await CreateShippingVesselAsync(oVessel, service)).WithTags("Shipping Vessel");
#endregion

#region shippingMethod - routes
app.MapGet("/ShippingMethod/List", async (IShippingService service) => await GetShippingMethodListAsync(service)).WithTags("Shipping Method");
app.MapPost("/ShippingMethod/Create", async (ShippingMethodLookup oMethod, IShippingService service) => await CreateShippingMethodAsync(oMethod, service)).WithTags("Shipping Method");
#endregion

#region shipper-category - routes
app.MapGet("/ShipperCategory/List", async (IShippingService service) => await GetShipperCategoriesAsync(service)).WithTags("Shipper Category");
app.MapPost("/ShipperCategory/Create", async (ShipperCategoryLookup oShipCategory, IShippingService service) => await CreateShipperCategoryAsync(oShipCategory, service)).WithTags("Shipper Category");
#endregion

#region DeliveryMethod - routes
app.MapGet("/DeliveryMethod/List", async (IShippingService service) => await GetDeliveryMethodListAsync(service)).WithTags("Delivery Method");
app.MapPost("/DeliveryMethod/Create", async (DeliveryMethodLookup deliveryMethod, IShippingService service) => await CreateDeliveryMethodAsync(deliveryMethod, service)).WithTags("Delivery Method");
#endregion

#region deliveryzone - routes
app.MapGet("/DeliveryZone/List",  (IShippingService service) =>  GetDeliveryZoneListAsync(service)).WithTags("Delivery Zone");
app.MapPost("/DeliveryZone/Create", async (DeliveryZoneLookup deliveryZone, IShippingService service) => await CreateDeliveryZoneAsync(deliveryZone, service)).WithTags("Delivery Zone");
#endregion

#region HScode - routes
app.MapGet("/HSCode/List", async (IShippingService service) => await GetHSCodeListAsync(service)).WithTags("HS Code");
app.MapPost("/HSCode/Create", async (HSCodeLookup hscode, IShippingService service) => await CreateHSCodeAsync(hscode, service)).WithTags("HS Code");
#endregion

#region insurance type routes 
app.MapGet("/InsuranceType/List", async (IShippingService service) => await GetInsuranceTypeListAsync(service)).WithTags("Insurance Type");
app.MapPost("/InsuranceType/Create", async (InsuranceTypeLookup insuranceType, IShippingService service) => await CreateInsuranceTypeAsync(insuranceType, service)).WithTags("Insurance Type");
#endregion

#region Insurance routes
app.MapGet("/Insurance/List",  (IShippingService service) =>  GetInsuranceListAsync(service)).WithTags("Insurance");
app.MapPost("/Insurance/Create", async (InsuranceLookup insurance, IShippingService service) => await CreateInsuranceAsync(insurance, service)).WithTags("Insurance");
#endregion

#region sailing schedule routes
app.MapGet("/SailingSchedule/List",  (IShippingService service) =>  GetSailingScheduleListAsync(service)).WithTags("Sailing Schedule");
app.MapPost("/SailingSchedule/Create", async (SailingScheduleLookup schedule, IShippingService service) => await CreateSailingScheduleAsync(schedule, service)).WithTags("Sailing Schedule");
#endregion

#region Packaging - routes

app.MapGet("/PackagingItem/Get", async (IPackagingService service) => await GetPackagingItemListAsync(service)).WithTags("PackagingItem");
app.MapGet("/PackagingPrice/Get", async (IPackagingService service) => await GetPackagingPriceListAsync(service)).WithTags("PackagingPrice");

app.MapPost("/PackagingItem/Create", async (PackageItemLookup oPackageItem, IPackagingService service) => await CreatePackagingItemAsync(oPackageItem, service)).WithTags("PackagingItem");
app.MapPost("/PackagingPrice/Create", async (PackagepriceLookup oPackagePrice, IPackagingService service) => await CreatePackagingPriceAsync(oPackagePrice, service)).WithTags("PackagingPrice");

#endregion

#region Seal - routes

app.MapGet("/SealType/List", async (ISealService service) => await GetSealTypeListAsync(service)).WithTags("SealType");
app.MapGet("/SealPrice/list", async (ISealService service) => await GetSealPriceListAsync(service)).WithTags("SealPrice");

app.MapPost("SealType/Create", async (SealTypeLookup oSealType, ISealService service) => await CreateSealTypeAsync(oSealType, service)).WithTags("SealType");
app.MapPost("SealPrice/Create", async (SealPriceLookup oSealPrice, ISealService service) => await CreateSealPriceAsync(oSealPrice, service)).WithTags("SealPrice");

#endregion

#region AdhocType - routes

app.MapGet("/Adhoc/List", async (IAdhocTypeService service) => await ListAdhocTypesAsync(service)).WithTags("AdhocType");
app.MapPost("/Adhoc/Create", async (AdhocTypeLookup oAdhoc, IAdhocTypeService service) => await CreateAdhocTypeAsync(oAdhoc, service)).WithTags("AdhocType");

#endregion

#region ContainerTypes - routes

app.MapGet("/ContainerType/Get", async (IContainerTypeService service) => await GetContainerTypesAsync(service)).WithTags("Container Types");
app.MapPost("/ContainerType/Create", async (IContainerTypeService service, ContainerTypeLookup oContainerType) => await CreateContainerTypeAsync(service, oContainerType)).WithTags("Container Types");

#endregion

#region DialCode - routes
app.MapGet("/DialCode/GetDialCodes", async (IDialCodeService service) => await GetDialAllDialCodesAsync(service)).WithTags("Dial Codes");
app.MapPost("/DialCode/Create", async (IDialCodeService service, DialCodeLookup oDialCode) => await CreateDialCodeAsync(service, oDialCode)).WithTags("Dial Codes");

#endregion

#region Airport - routes

app.MapGet("/Airport/GetAirports", async (IAirportService service) => await GetAllAirportsAsync(service)).WithTags("Airport");
app.MapPost("/Airport/Create", async (IAirportService service, AirportLookup oAirport) => await CreateAirportAsync(service, oAirport)).WithTags("Airport");
#endregion

#region Department - routes

app.MapGet("/Department/GetDepartments", async (IDepartmentService service) => await GetDepartmentsAsync(service)).WithTags("Department");
app.MapPost("/Department/CreateDepartment", async (DepartmentLookup oDepartment, IDepartmentService service) => await CreateDepartmentAsync(oDepartment, service)).WithTags("Department");
app.MapPut("/Department/UpdateDepartment", async (DepartmentLookup oDepartment, IDepartmentService service) => await UpdateDepartmentAsync(oDepartment, service)).WithTags("Department");
app.MapPost("/Department/Upload", async (List<DepartmentLookup> departmentList, IDepartmentService service) => await UploadDepartmentAsync(departmentList, service)).WithTags("Department");

#endregion

#region user - routes

app.MapGet("/User/GetUsers", async (IUserService usrservice) => await GetUsersAsync(usrservice)).WithTags("Authentication");
app.MapPost("/User/GetMD5Encryption", async (SingleParam userData, IUserService usrservice) => await GetMD5Encryption(userData, usrservice)).WithTags("Authentication");
app.MapPost("/User/AuthenticateUser", async (UserInfo userCredential, IUserService usrservice) => await AuthenticateUserAsync(userCredential, usrservice)).WithTags("Authentication");
app.MapPost("/User/SetLoginFlag", async (UserInfo user, IUserService service) => await SetLoginFlagAsync(user, service)).WithTags("Authentication");
app.MapPost("/User/SetLogoutFlag", async (UserInfo user, IUserService service) => await SetLogoutFlagAsync(user, service)).WithTags("Authentication");
app.MapPost("/User/GetUserProfile", async (UserInfo _user, IUserService service) => await GetUserProfileAsync(_user, service)).WithTags("Authentication");
app.MapPost("/User/AmendUserProfile", async (UserProfile userProfile, IUserService service) => await AmendUserProfileAsync(userProfile, service)).WithTags("Authentication");
app.MapPost("/User/CreateAccount", async (userRecord _userRecord, IUserService service) => await CreateUserAccountAsync(_userRecord, service)).WithTags("Authentication");

app.MapPut("/User/ChangePassword", async (UserInfo oUserInfo, IUserService service) => await ChangePasswordAsync(oUserInfo, service)).WithTags("Authentication");

#endregion

#region Log-routes

app.MapPost("/Log/WriteLog", async (Log oLogger, ILoggerService service) => await WriteLogAsync(oLogger, service)).WithTags("Logger");
app.MapGet("/Log/GetLogs", async (ILoggerService service) => await GetLogDataAsync(service)).WithTags("Logger");
app.MapPost("/Log/GetUserLogs", async (SingleParam _param, ILoggerService service) => await GetUserLogsAsync(_param, service)).WithTags("Logger");

#endregion

#region Profile-routes

app.MapPost("/Profile/CreateProfile", async (SystemProfile oProfile, IProfileService service) => await CreateProfileAsync(oProfile, service)).WithTags("Profile");
app.MapPost("/Profile/AmendProfile", async (SystemProfile oProfile, IProfileService service) => await AmendProfileAsync(oProfile, service)).WithTags("Profile");
app.MapPost("/Profile/GetProfiles", async (SingleParam companyIdentifier, IProfileService service) => await GetProfileListAsync(companyIdentifier, service)).WithTags("Profile");
app.MapGet("/Profile/GetAllProfiles", async (IProfileService service) => await GetAllProfilesAsync(service)).WithTags("Profile");
app.MapPost("/Profile/GetProfileModules", async (SingleParam oProfileObj, IProfileService service) => await GetProfileModulesAsync(oProfileObj, service)).WithTags("Profile");

#endregion

#region city endpoints

app.MapGet("/City/GetCities", async (ICityService service) => await GetCitiesAsync(service)).WithTags("City");
app.MapGet("/City/GetActiveCities", async (ICityService service) => await GetActiveCitiesAsync(service)).WithTags("City");
app.MapPost("/City/CreateCity", async (CityLookup oCity, ICityService service) => await CreateCityAsync(oCity, service)).WithTags("City");
app.MapPut("/City/UpdateCity", async (CityLookup oCity, ICityService service) => await UpdateCityAsync(oCity, service)).WithTags("City");
app.MapPut("/City/UpdateCountryOfCity", async (CityLookup oCity, ICityService service) => await UpdateCountryOfCityAsync(oCity, service)).WithTags("City");
app.MapPost("/City/Upload", async (List<CityLookup> cityList, ICityService service) => await UploadCitiesAsync(cityList, service)).WithTags("City");

#endregion

#region country - routes

app.MapGet("/Country/GetCountries", async (ICountryService service) => await GetCountriesAsync(service)).WithTags("Country");
app.MapPost("/Country/CreateCountry", async (CountryLookup oCountry, ICountryService service) => await CreateCountryAsync(oCountry, service)).WithTags("Country");
app.MapPut("/Country/UpdateCountry", async (CountryLookup oCountry, ICountryService service) => await UpdateCountryAsync(oCountry, service)).WithTags("Country");
app.MapPost("/Country/Upload", async (List<CountryLookup> countryList, ICountryService service) => await UploadCountriesAsync(countryList, service)).WithTags("Country");
#endregion

#region Referral-routes

app.MapGet("/Referral/GetReferrals", async (IReferralService service) => await GetReferralsAsync(service)).WithTags("Referral");
app.MapPost("/Referral/CreateReferral", async (ReferralLookup oReferral, IReferralService service) => await CreateReferralAsync(oReferral, service)).WithTags("Referral");
app.MapPut("/Referral/UpdateReferral", async (ReferralLookup oReferral, IReferralService service) => await UpdateReferralAsync(oReferral, service)).WithTags("Referral");

#endregion

#region companyType - routes

app.MapGet("/CompanyType/GetCompanyTypes", async (ICompanyService service) => await GetCompanyTypesAsync(service)).WithTags("CompanyType");
app.MapPost("/CompanyType/CreateCompanyType", async (CompanyTypeLookup oCompanyType, ICompanyService service) => await CreateCompanyTypeAsync(oCompanyType, service)).WithTags("CompanyType");
app.MapPut("/CompanyType/UpdateCompanyType", async (CompanyTypeLookup oCompanyType, ICompanyService service) => await UpdateCompanyTypeAsync(oCompanyType, service)).WithTags("CompanyType");

app.MapGet("/Company/Get", async (ICompanyService service) => await GetCompaniesAsync(service)).WithTags("Company");
#endregion

#region Modules - routes

app.MapGet("/Module/GetModulesInUse", async (IModuleService service) => await GetModulesInUseAsync(service)).WithTags("User Module");
app.MapGet("/Module/GetAllModules", async (IModuleService service) => await GetAllModulesAsync(service)).WithTags("User Module");

#endregion

#region Region-routes

app.MapGet("/Region/Get", async (IRegionService service) => await GetRegionsAsync(service)).WithTags("Region");
app.MapPost("/Region/Create", async (IRegionService service, RegionLookup oRegion) => await CreateRegionAsync(service, oRegion)).WithTags("Region");

#endregion

#region Branch - routes

app.MapGet("/Branch/Get", async (IBranchService service) => await GetBranchesAsync(service)).WithTags("Branch");
app.MapPost("/Branch/Create", async (BranchLookup oBranch, IBranchService service) => await CreateBranchAsync(oBranch, service)).WithTags("Branch");
app.MapPost("/Branch/Upload", async (List<BranchLookup> branchList, IBranchService service) => await UploadBranchAsync(branchList, service)).WithTags("Branch");
#endregion

#region Title - routes

app.MapGet("/Title/Get", async (ITitleService service) => await GetTitlesAsync(service)).WithTags("Titles");
app.MapPost("/Title/Create", async (TitleLookup oTitle, ITitleService service) => await CreateTitleAsync(oTitle, service)).WithTags("Titles");
app.MapPost("/Title/Upload", async (List<TitleLookup> titleList, ITitleService service) => await UploadTitleDataAsync(titleList, service)).WithTags("Titles");
#endregion

#region ShippingPorts - routes

app.MapPost("/ShippingPort/Create", async (ShippingPortLookup oShippingPort, IShippingPortService service) => await CreateShippingPortAsync(oShippingPort, service)).WithTags("Shipping Port");
app.MapGet("/ShippingPort/List", (IShippingPortService service) => GetShippingPortListAsync(service)).WithTags("Shipping Port");

#endregion

#region tasks

async Task<IResult> CreateSailingScheduleAsync(SailingScheduleLookup schedule, IShippingService service)
{
    if ((schedule.oVessel.id == 0) || (schedule.oDeparturePort.id == 0) || (schedule.oArrivalPort.id == 0) || (schedule.dateOfarrival < schedule.dateOfdeparture))
        return Results.BadRequest(@"Unacceptable data");

    try
    {
        var opStatus = await service.CreateSailingScheduleAsync(schedule);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateInsuranceAsync(InsuranceLookup insurance, IShippingService service)
{
    if ((insurance.oInsuranceType.id == 0) || (insurance.unitPrice == 0m) || (insurance.insuranceDescription.Length < 1))
        return Results.BadRequest(@"insurance type, unit price and description cannot be blank");

    try
    {
        var opStatus = await service.CreateInsuranceAsync(insurance);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateInsuranceTypeAsync(InsuranceTypeLookup insuranceType, IShippingService service)
{
    if (insuranceType.insuranceType.Length < 1)
        return Results.BadRequest(@"insurance type cannot be blank");

    try
    {
        var opStatus = await service.CreateInsuranceTypeAsync(insuranceType);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateHSCodeAsync(HSCodeLookup hscode, IShippingService service)
{
    if ((hscode.code.Length < 1) || (hscode.description.Length < 1))
        return Results.BadRequest(@"hs code and description cannot be blank");

    try
    {
        var opStatus = await service.CreateHSCodAsync(hscode);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateDeliveryZoneAsync(DeliveryZoneLookup deliveryZone, IShippingService service)
{
    if ((deliveryZone.oDeliveryMethod.id == 0) || (deliveryZone.zoneName.Length < 1) || (deliveryZone.oCountry.id == 0))
        return Results.BadRequest(@"delivery method, zone name and country cannot be blank");

    try
    {
        var opStatus = await service.CreateDeliveryZoneAsync(deliveryZone);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateDeliveryMethodAsync(DeliveryMethodLookup deliveryMethod, IShippingService service)
{
    if ((deliveryMethod.method.Length < 1) || (deliveryMethod.methodDescription.Length < 1))
        return Results.BadRequest(@"deliver method and delivery description cannot be blank");

    try
    {
        var opStatus = await service.CreateDeliveryMethodAsync(deliveryMethod);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateShipperCategoryAsync(ShipperCategoryLookup oShipCategory, IShippingService service)
{
    if (oShipCategory.description.Length < 1)
        return Results.BadRequest(@"shipper category description cannot be blank");

    try
    {
        var opStatus = await service.CreateShipperCategoryAsync(oShipCategory);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateShippingMethodAsync(ShippingMethodLookup oMethod,IShippingService service)
{
    if ((oMethod.shippingMethod.Length < 1) || (oMethod.shippingRoute.Length < 1))
        return Results.BadRequest(@"shipping method and shipping routes cannot have blank values");

    try
    {
        var opStatus = await service.CreateShippingMethodAsync(oMethod);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateShippingVesselAsync(VesselLookup oVessel,IShippingService service)
{
    if ((oVessel.nameOfvessel.Length < 1) || (oVessel.flagOfvessel.Length < 1))
        return Results.BadRequest(@"name of vessel and flag of vessel cannot be blank");

    if (oVessel.oShippingLine.id == 0)
        return Results.BadRequest(@"Id of shipping line cannot be zero (0)");

    try
    {
        var opStatus = await service.CreateShippingVesselAsync(oVessel);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateShippingLineAsync(ShippingLineLookup oShippingLine, IShippingService service)
{
    if (oShippingLine.shippingLine.Length < 1)
        return Results.BadRequest(@"shipping line cannot be blank");

    try
    {
        var opStatus = await service.CreateShippingLineAsync(oShippingLine);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
IResult GetShippingPortListAsync(IShippingPortService service)
{
    try
    {
        var shipping_port_list = service.GetShippingPortsAsync();
        return Results.Ok(shipping_port_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
IResult GetSailingScheduleListAsync(IShippingService service)
{
    try
    {
        var sailing_schedule_list =  service.GetSailingScheduleListAsync();
        return Results.Ok(sailing_schedule_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
IResult GetInsuranceListAsync(IShippingService service)
{
    try
    {
        var insurance_list = service.GetInsuranceListAsync();
        return Results.Ok(insurance_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetInsuranceTypeListAsync(IShippingService service)
{
    try
    {
        var insurance_type_list = await service.GetInsuranceTypeListAsync();
        return Results.Ok(insurance_type_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetHSCodeListAsync(IShippingService service)
{
    try
    {
        var code_list = await service.GetHSCodeListAsync();
        return Results.Ok(code_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
IResult GetDeliveryZoneListAsync(IShippingService service)
{
    try
    {
        var delivery_zone_list = service.GetDeliveryZoneListAsync();
        return Results.Ok(delivery_zone_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetDeliveryMethodListAsync(IShippingService service)
{
    try
    {
        var delivery_methods = await service.GetDeliveryMethodListAsync();
        return Results.Ok(delivery_methods);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetShippingLineListAsync(IShippingService service)
{
    try
    {
        var shipping_line_list = await service.GetShippingLineListAsync();
        return Results.Ok(shipping_line_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
IResult GetShippingVesselListAsync(IShippingService service)
{
    try
    {
        var vessel_list = service.GetShippingVesselListAsync();
        return Results.Ok(vessel_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetShippingMethodListAsync(IShippingService service)
{
    try
    {
        var ship_method_list = await service.GetShippingMethodListAsync();
        return Results.Ok(ship_method_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetShipperCategoriesAsync(IShippingService service)
{
    try
    {
        var ship_category_list = await service.GetShipperCategoryListAsync();
        return Results.Ok(ship_category_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion



#region api - tasks

#region Seal - tasks

async Task<IResult> CreateSealPriceAsync(SealPriceLookup oSealPrice, ISealService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oSealPrice.oSealType.id == 0)
        return Results.BadRequest(@"seal type ID cannot be zero (0)");
    if (oSealPrice.sellingPrice <= 0)
        return Results.BadRequest(@"selling price of seal must be greater than zero (0)");
    if (oSealPrice.Price <= 0)
        return Results.BadRequest(@"price of seal must be greater than zero (0)");

    try
    {
        var opStatus = await service.CreateSealPriceAsync(oSealPrice);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateSealTypeAsync(SealTypeLookup oSealType, ISealService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oSealType.sealDescription.Length < 1)
        return Results.BadRequest(@"seal type name cannot be blank");

    try
    {
        var opStatus = await service.CreateSealTypeAsync(oSealType);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetSealTypeListAsync(ISealService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var seal_type_list = await service.GetSealTypeListAsync();
        return Results.Ok(seal_type_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetSealPriceListAsync(ISealService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var seal_price_list = await service.GetSealPriceListAsync();
        return Results.Ok(seal_price_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
#endregion

#region Packaging - tasks
async Task<IResult> GetPackagingItemListAsync(IPackagingService service)
{
    //get packaging item list
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var packaging_item_list = await service.GetPackageItemListAsync();
        return Results.Ok(packaging_item_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetPackagingPriceListAsync(IPackagingService service)
{
    //gets packaging price list
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var packaging_price_list = await service.GetPackagePriceListAsync();
        return Results.Ok(packaging_price_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> CreatePackagingItemAsync(PackageItemLookup oPackageItem, IPackagingService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oPackageItem.name.Length <= 0)
        return Results.BadRequest(@"name of package item cannot be blank");

    try
    {
        var opStatus = await service.CreatePackageItemAsync(oPackageItem);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> CreatePackagingPriceAsync(PackagepriceLookup oPackagePrice, IPackagingService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oPackagePrice.unitPrice <= 0)
        return Results.BadRequest(@"unit price cannot be zero (0)");

    if (oPackagePrice.wholesalePrice <= 0)
        return Results.BadRequest(@"wholesale price cannot be zero (0)");

    if (oPackagePrice.retailPrice <= 0)
        return Results.BadRequest(@"retail price cannot be zero (0)");

    try
    {
        var opStatus = await service.CreatePackagingPriceAsync(oPackagePrice);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion

#region Adhoc - tasks

async Task<IResult> ListAdhocTypesAsync(IAdhocTypeService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var adhoc_type_list = await service.GetAdHocTypesAsync();
        return Results.Ok(adhoc_type_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateAdhocTypeAsync(AdhocTypeLookup oAdhoc, IAdhocTypeService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if ((oAdhoc.name.Length < 1) || (oAdhoc.nomCode.Length < 1))
        return Results.BadRequest(@"Adhoc values cannot be blank");

    try
    {
        var opStatus = await service.CreateAdhocTypeAsync(oAdhoc);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
#endregion

#region Container-type - tasks

async Task<IResult> CreateContainerTypeAsync(IContainerTypeService service, ContainerTypeLookup oContainerType)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oContainerType.containerType.Length < 1)
        return Results.BadRequest(@"container type cannot be blank");

    if (oContainerType.containerVolume == 0m)
        return Results.BadRequest(@"volume of container cannot be zero (0)");

    try
    {
        var opStatus = await service.CreateContainerTypeAsync(oContainerType);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetContainerTypesAsync(IContainerTypeService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var containertypeList = await service.GetContainerTypesAsync();
        return Results.Ok(containertypeList);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion

#region DialCodes - tasks

async Task<IResult> CreateDialCodeAsync(IDialCodeService service, DialCodeLookup oDialCode)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oDialCode.dialCode.Length < 1)
        return Results.BadRequest(@"dial code cannot be blank");

    if (oDialCode.oCountry.id < 1)
        return Results.BadRequest(@"country Id cannot be zero (0)");

    try
    {
        var opStatus = await service.CreateDialCodeAsync(oDialCode);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> GetDialAllDialCodesAsync(IDialCodeService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var dialcode_list = await service.GetDialCodesAsync();
        return Results.Ok(dialcode_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion

#region Airport - tasks

async Task<IResult> GetAllAirportsAsync(IAirportService service)
{
    if (service == null)
        return Results.BadRequest(@"service cannot be instantiated");

    try
    {
        var results = await service.GetAirportsAsync();
        return Results.Ok(results);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> CreateAirportAsync(IAirportService service, AirportLookup oAirport)
{
    if (service == null)
        return Results.BadRequest(@"service cannot be instantiated");

    if (oAirport.nameOfairport.Length < 1)
        return Results.BadRequest(@"airport name cannot be blank");

    if (oAirport.airportMnemonic.Length < 1)
        return Results.BadRequest(@"airport code cannot be blank");
    
    if (oAirport.oCountry.id < 1)
        return Results.BadRequest(@"country Id cannot be zero (0)");

    try
    {
        var opStatus = await service.CreateAirportAsync(oAirport);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
#endregion

#region ShippingPort - tasks

async Task<IResult> CreateShippingPortAsync(ShippingPortLookup oShippingPort, IShippingPortService service)
{
    if (oShippingPort.nameOfport.Trim().Length < 1)
        return Results.BadRequest(@"port name cannot be blank");

    if (oShippingPort.codeOfport.Trim().Length < 1)
        return Results.BadRequest(@"port code cannot be blank");


    if (oShippingPort.oCountry.id < 1)
        return Results.BadRequest(@"ID of country cannot be zero (0)");

    try
    {
        var opStatus = await service.CreateShippingPortAsync(oShippingPort);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion

#region Title - tasks

async Task<IResult> GetTitlesAsync(ITitleService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");
    try
    {
        var title_List = await service.GetTitlesAsync();
        return Results.Ok(title_List);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
async Task<IResult> CreateTitleAsync(TitleLookup oTitle, ITitleService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oTitle.nameOftitle.Trim().Length < 1)
        return Results.BadRequest(@"Title cannot be blank");
    
    try
    {
        var opStatus = await service.CreateTitleAsync(oTitle);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> UploadTitleDataAsync(List<TitleLookup> titleList,ITitleService service)
{
    try
    {
        if ((titleList == null) || (titleList.Count() == 0))
            return Results.NoContent();

        if (service == null)
            return Results.BadRequest(@"service could not be instantiated");

        var opStatus = await service.UploadTitleAsync(titleList);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}
#endregion

#region Branch - tasks

async Task<IResult> GetBranchesAsync(IBranchService service)
{
    if (service == null)
        return Results.BadRequest(@"service cannot be instantiated");

    try
    {
        var branch_list = await service.GetBranchesAsync();
        return Results.Ok(branch_list);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> CreateBranchAsync(BranchLookup oBranch, IBranchService service)
{
    if (service == null)
        return Results.BadRequest(@"service cannot be instantiated");

    if (oBranch == null)
        return Results.BadRequest(@"payLoad cannot be null");

    if (oBranch.nameOfbranch == string.Empty)
        return Results.BadRequest(@"name of branch cannot be blank");

    if (oBranch.oCompany.id < 1)
        return Results.BadRequest(@"Id of company cannot be zero (0)");

    var opStatus = await service.CreateBranchAsync(oBranch);
    return Results.Ok(opStatus);
}

async Task<IResult> UploadBranchAsync(List<BranchLookup> branchList, IBranchService service)
{
    try
    {
        if ((branchList == null) && (branchList.Count() < 1))
            return Results.NoContent();

        if (service == null)
            return Results.BadRequest(@"service cannot be instantiated");

        var opStatus = await service.UploadBranchAsync(branchList);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion

#region Department - tasks
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

async Task<IResult> CreateDepartmentAsync(DepartmentLookup oDepartment, IDepartmentService service)
{
    try
    {
        if (oDepartment.nameOfdepartment == string.Empty)
            return Results.BadRequest(@"department name cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service was not instantiated");

        var dept_create_status = await service.CreateDepartmentAsync(oDepartment);
        return Results.Ok(dept_create_status);
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex);
    }
}

async Task<IResult> UpdateDepartmentAsync(DepartmentLookup oDepartment, IDepartmentService service)
{
    try
    {
        if (oDepartment.nameOfdepartment == string.Empty)
            return Results.BadRequest(@"department name cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service was not instantiated");

        var dept_update_status = await service.UpdateDepartmentAsync(oDepartment);
        return Results.Ok(dept_update_status);
    }
    catch(Exception apiErr)
    {
        return Results.BadRequest(apiErr);
    }
}

async Task<IResult> UploadDepartmentAsync(List<DepartmentLookup> departmentList, IDepartmentService service)
{
    //uploads the department
    try
    {
        if ((departmentList == null) && (departmentList.Count() < 1))
            return Results.NotFound();

        if (service == null)
            return Results.BadRequest(@"service cannot be instantiated");

        var opStatus = await service.UploadDepartmentAsync(departmentList);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

#endregion

#region User - tasks

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
    catch (Exception ex)
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
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
}

async Task<IResult> ChangePasswordAsync(UserInfo oUserInfo, IUserService service)
{
    //amends the password of a user resource
    try
    {
        if (oUserInfo.username == string.Empty)
            return Results.BadRequest(@"username cannot be blank");

        if (oUserInfo.password == string.Empty)
            return Results.BadRequest(@"user password cannot be blank");

        //implementation
        var operationStatus = await service.ChangeUserPasswordAsync(oUserInfo);
        return Results.Ok(operationStatus);
    }
    catch (Exception x)
    {
        return Results.BadRequest($"error: {x.Message}");
    }
}

#endregion

#region Logger - tasks
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
    catch (Exception ex)
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
    catch (Exception ex)
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
    catch (Exception ex)
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
    catch (Exception x)
    {
        return Results.BadRequest($"error: {x.Message}");
    }
}

#endregion

#region Profile - tasks

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
    catch (Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

async Task<IResult> AmendProfileAsync(SystemProfile oProfile, IProfileService service)
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
    catch (Exception ex)
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
    catch (Exception x)
    {
        return Results.BadRequest(x.Message);
    }

}

async Task<IResult> GetAllProfilesAsync(IProfileService service)
{
    try
    {
        if (service == null)
            return Results.BadRequest(@"service not instantiated");

        var response = await service.GetProfilesAsync();
        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
}
async Task<IResult> GetUserProfileAsync(UserInfo _user, IUserService service)
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
    catch (Exception ex)
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
    catch (Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}

#endregion

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

#region City Implementation

async Task<IResult> GetCitiesAsync(ICityService service)
{
    try
    {
        if (service == null)
            return Results.BadRequest(@"service could not be instantiated");

        var dta = await service.GetAllCitiesAsync();
        return Results.Ok(dta);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> GetActiveCitiesAsync(ICityService service)
{
    try
    {
        if (service == null)
            return Results.BadRequest(@"service could not be instantiated");

        var active_city_list = await service.GetCitiesAsync();
        return Results.Ok(active_city_list);
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex);
    }
}
async Task<IResult> CreateCityAsync(CityLookup oCity, ICityService service)
{
    try
    {
        if (oCity.nameOfcity == string.Empty)
            return Results.BadRequest(@"name of city cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service not instantiated");

        //consume service
        var resp = await service.CreateCityAsync(oCity);
        return Results.Ok(resp);
    }
    catch(Exception err)
    {
        return Results.BadRequest(err);
    }
}

async Task<IResult> UpdateCityAsync(CityLookup oCity, ICityService service)
{
    //endpoint updates city record, specifically the city name
    try
    {
        if (oCity.nameOfcity == string.Empty)
            return Results.BadRequest(@"name of city cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service not instantiated");

        var update_status = await service.UpdateCityAsync(oCity);
        return Results.Ok(update_status);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> UpdateCountryOfCityAsync(CityLookup oCity, ICityService service)
{
    //endpoint updates the country of the city
    try
    {
        if (oCity.nameOfcity == string.Empty)
            return Results.BadRequest(@"name of city cannot be blank");

        if (oCity.oCountry.nameOfcountry == string.Empty)
            return Results.BadRequest(@"name of country cannot be blank");

        var country_update_status = await service.UpdateCountryOfCityAsync(oCity);
        return Results.Ok(country_update_status);
    }
    catch(Exception err)
    {
        return Results.BadRequest(err);
    }
}

async Task<IResult> UploadCountriesAsync(List<CountryLookup> countryList, ICountryService service)
{
    try
    {
        if ((countryList == null) || (countryList.Count() < 1))
            return Results.NoContent();

        if (service == null)
            return Results.BadRequest(@"service could not be instantiated");

        var opStatus = await service.UploadCountryAsync(countryList);
        return Results.Ok(opStatus);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> UploadCitiesAsync(List<CityLookup> cityList, ICityService service)
{
    //endpoint updates city data store through uploads
    //implements idompotency
    try
    {
        if (cityList == null)
            return Results.NoContent();

        if (cityList.Count() < 1)
            return Results.NoContent();

        if (service == null)
            return Results.BadRequest(@"service could not be instantiated");

        var opStatus = await service.UploadCityDataAsync(cityList);
        return Results.Ok(opStatus);
    }
    catch(Exception err)
    {
        return Results.BadRequest(err);
    }
}

#endregion

#region Country - tasks

async Task<IResult> GetCountriesAsync(ICountryService service)
{
    //gets countries
    try
    {
        if (service == null)
            return Results.BadRequest(@"service has not been instantiated");

        var dta = await service.GetCountriesAsync();
        return Results.Ok(dta);
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex);
    }
}
async Task<IResult> CreateCountryAsync(CountryLookup oCountry, ICountryService service)
{
    try
    {
        swContext cfg = new swContext();
        if (oCountry.nameOfcountry == string.Empty)
            return Results.BadRequest(@"name of country cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service was not instantiated");

            var status = await service.CreateCountryAsync(oCountry);
            return Results.Ok(status);
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex);
    }
}

async Task<IResult> UpdateCountryAsync(CountryLookup oCountry, ICountryService service)
{
    try
    {
        swContext config = new swContext();

        var r = await config.TRegionLookups.Where(x => x.RegionName == oCountry.oRegion.nameOfregion.Trim()).FirstOrDefaultAsync();

        if (r != null)
        {
            oCountry.oRegion.id = r.RegionId;

            var update_status = await service.UpdateCountryAsync(oCountry);
            return Results.Ok(update_status);
        }
        else { return Results.BadRequest(@"region Id cannot be zero (0)"); }
    }
    catch(Exception exc)
    {
        return Results.BadRequest(exc);
    }
}

#endregion

#region Referral-Tasks

async Task<IResult> GetReferralsAsync(IReferralService service)
{
    //endpoint for getting list of referrals from the data store
    try
    {
        if (service == null)
            return Results.BadRequest(@"service was not instantiated");

        var data_ = await service.getReferralsAsync();
        return Results.Ok(data_);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}
async Task<IResult> CreateReferralAsync(ReferralLookup oReferral, IReferralService service)
{
    //creates a new referral resource in the data store
    try
    {
        if (oReferral.sourceOfReferral == string.Empty)
            return Results.BadRequest(@"source of referral cannot be blank");
        if (service == null)
            return Results.BadRequest(@"service was not instantiated");

        var status = await service.CreateReferralAsync(oReferral);
        return Results.Ok(status);
    }
    catch(Exception exc)
    {
        return Results.BadRequest($"error: {exc.Message}");
    }
}
async Task<IResult> UpdateReferralAsync(ReferralLookup oReferral, IReferralService service)
{
    //endpoint updates a referral record in the data store
    try
    {
        if (oReferral.id < 1)
            return Results.BadRequest(@"Id of Referral cannot be zero (0)");

        if (oReferral.sourceOfReferral == string.Empty)
            return Results.BadRequest(@"source of referral cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service was not instantiated");

        var updateObj = await service.UpdateReferralAsync(oReferral);
        return Results.Ok(updateObj);
    }
    catch(Exception x)
    {
        return Results.BadRequest($"error: {x.Message}");
    }
}
#endregion

#region companyType -tasks

async Task<IResult> GetCompanyTypesAsync(ICompanyService service)
{
    //retrieves resources in the data store
    try
    {
        if (service == null)
            return Results.BadRequest(@"service is not instantiated");

        var companyTypeDta = await service.GetCompanyTypesAsync();
        return Results.Ok(companyTypeDta);
    }
    catch(Exception x)
    {
        return Results.BadRequest($"error: {x.Message}");
    }
}
async Task<IResult> CreateCompanyTypeAsync(CompanyTypeLookup oCompanyType, ICompanyService service)
{
    //creates a company type resource
    if (oCompanyType.typeOfCompany == string.Empty)
        return Results.BadRequest(@"type of company cannot be blank");

    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    var operationStatus = await service.CreateCompanyTypeAsync(oCompanyType);
    return Results.Ok(operationStatus);
}
async Task<IResult> UpdateCompanyTypeAsync(CompanyTypeLookup oCompanyType, ICompanyService service)
{
    //modifies an already existing resource
    try
    {
        if (oCompanyType.id < 1)
            return Results.BadRequest(@"Id of company type cannot be zero (0)");

        if (oCompanyType.typeOfCompany == string.Empty)
            return Results.BadRequest(@"type of company cannot be blank");

        if (service == null)
            return Results.BadRequest(@"service could not be instantiated");

        var opStatus = await service.UpdateCompanyTypeAsync(oCompanyType);
        return Results.Ok(opStatus);
    }
    catch(Exception ex)
    {
        return Results.BadRequest($"error: {ex.Message}");
    }
}
#endregion

#region Company - Tasks

async Task<IResult> GetCompaniesAsync(ICompanyService service)
{
    //gets companies from the data store
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var company_list = await service.GetCompaniesAsync();
        return Results.Ok(company_list);
    }
    catch(Exception exc)
    {
        return Results.BadRequest(exc);
    }
}

#endregion

#region Modules - Tasks

async Task<IResult> GetModulesInUseAsync(IModuleService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var moduleDta = await service.GetModulesInUseAsync();
        return Results.Ok(moduleDta);
    }
    catch(Exception x)
    {
        return Results.BadRequest(x);
    }
}

async Task<IResult> GetAllModulesAsync(IModuleService service)
{
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var all_moduleDta = await service.GetModulesAsync();
        return Results.Ok(all_moduleDta);
    }
    catch (Exception x)
    {
        return Results.BadRequest(x);
    }
}
#endregion

#region Region - Tasks

async Task<IResult> GetRegionsAsync(IRegionService service)
{
    //gets region
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    try
    {
        var regionList = await service.GetRegionAsync();
        return Results.Ok(regionList);
    }
    catch(Exception exc)
    {
        return Results.BadRequest(exc);
    }
}

async Task<IResult> CreateRegionAsync(IRegionService service, RegionLookup oRegion)
{
    //creates region
    if (service == null)
        return Results.BadRequest(@"service could not be instantiated");

    if (oRegion == null)
        return Results.BadRequest(@"region cannot be instantiated");

    if (oRegion.nameOfregion == string.Empty)
        return Results.NotFound(@"region name cannot be blank");

    try
    {
        var opStatus = await service.CreateRegionAsync(oRegion);
        return Results.Ok(opStatus);
    }
    catch(Exception exc)
    {
        return Results.BadRequest(exc);
    }
}

#endregion

#endregion

#endregion

app.Run();