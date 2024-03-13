#nullable disable
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.utils;
using System.Drawing.Printing;
using Xero.NetStandard.OAuth2.Model.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Resources.Implementations
{
    public class VehicleService : IVehicleService
    {
        swContext config;

        public VehicleService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> SaveVehicleAsync(clsVehicle payLoad)
        {
            //TODO: save vehicle record into the data store
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var opStatus = await helper.CreateVehicleRecordAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = opStatus,
                    message = opStatus == true ? $"Vehicle with registration No. {payLoad.registrationNo} saved successfully!!!" : $"An error occured while saving vehicle with registration No {payLoad.registrationNo}. Plaese contact the Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
    
        public async Task<PaginationAPIResponse> ListVehiclesAsync(int page, int pageSize)
        {
            //TODO: gets the list of vehicles from the data store
            PaginationAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var vehicleData = await helper.ListVehiclesAsync();

                //paginating data
                var totalCount = vehicleData.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                response = new PaginationAPIResponse()
                {
                    status = vehicleData.Count() > 0 ? true: false,
                    message = vehicleData.Count() > 0 ? @"success": @"failed",
                    total = totalCount,
                    data = vehicleData.Skip((page -1) * pageSize).Take(pageSize).ToList()
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetUnassignedVehiclesAsync()
        {
            //TODO: gets all unassigned vehicles without pagination
            DefaultAPIResponse rsp = null;
            List<clsVehicle> vehicles = new List<clsVehicle>();

            try
            {
                Helper helper = new Helper();
                var vehicleData = await helper.ListVehiclesAsync();

                foreach(var vehicle in vehicleData)
                {
                    if (vehicle.isAssigned == @"No")
                    {
                        vehicles.Add(vehicle);
                    }
                }

                return rsp = new DefaultAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = vehicles.ToList()
                };
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> ListUnassignedVehiclesAsync(int page, int pageSize)
        {
            //TODO: gets unassigned vehicle list
            PaginationAPIResponse rsp = null;
            List<clsVehicle> vehicleList = null;

            try
            {
                IVehicleService i_service = new VehicleService();
                var dta = await i_service.ListVehiclesAsync(page, pageSize);

                if (dta.status)
                {
                    vehicleList = new List<clsVehicle>();
                    foreach(var item in (List<clsVehicle>)dta.data)
                    {
                        if (item.isAssigned == @"No")
                        {
                            vehicleList.Add(item);
                        }
                    }

                    //paginating data
                    var totalCount = vehicleList.Count();
                    var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                    rsp = new PaginationAPIResponse()
                    {
                        status = totalCount > 0 ? true : false,
                        message = totalCount > 0 ? $"Page {page} out of {totalPages} fetched" : @"failed",
                        total = totalCount,
                        data = vehicleList.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                    };
                }

                return rsp;
            }
            catch(Exception e)
            {
                return rsp = new PaginationAPIResponse() { 
                    status = false,
                    message = $"error: {e.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> GetUnassignedDriversAsync(List<userRecord> userRecords, int page, int pageSize)
        {
            //gets unassigned drivers from the data store
            PaginationAPIResponse response = null;
            List<userRecord> unassigned = new List<userRecord>();

            try
            {
                Helper helper = new Helper();
                foreach(var ur in userRecords)
                {
                    bool bn = await new Helper() { }.getDriverAssignmentStatus(ur);
                    if (! bn)
                    {
                        //has not been assigned 
                        unassigned.Add(ur);
                    }
                }

                var totalCount = unassigned.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                return response = new PaginationAPIResponse() { 
                    status = totalCount > 0 ? true: false,
                    message = totalCount > 0 ? $"Page {page} out of {totalPages}: {pageSize} out of {totalCount} records fetched": @"failed",
                    total = totalCount,
                    data = unassigned.Skip((page -1) * pageSize).Take(pageSize).ToList(), 
                };
            }
            catch(Exception x)
            {
                return response = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetALLDriversUnassigned(List<userRecord> userRecords)
        {
            //TODO: Gets all drivers unassigned
            DefaultAPIResponse response = null;
            List<userRecord> unassigned = new List<userRecord>();

            try
            {
                Helper helper = new Helper();
                foreach (var ur in userRecords)
                {
                    bool bn = await new Helper() { }.getDriverAssignmentStatus(ur);
                    if (!bn)
                    {
                        //has not been assigned 
                        unassigned.Add(ur);
                    }
                }
                var totalCount = unassigned.ToList().Count();

                return response = new DefaultAPIResponse() { 
                    status = totalCount > 0? true: false,
                    message = totalCount > 0 ? $"{totalCount} records fetched": @"No data",
                    data = unassigned.ToList()
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> GetAssignedDriversAsync(int page, int pageSize)
        {
            //gets unassigned drivers from the data store
            PaginationAPIResponse response = null;
            List<DriverVehicleRecord> assigned = new List<DriverVehicleRecord>();

            try
            {
                Helper helper = new Helper();
                var dList = await helper.GetAssignedDriversAsync();
                assigned = dList.ToList();

                var totalCount = assigned.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                return response = new PaginationAPIResponse()
                {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? $"Page {page} out of {totalPages}: {pageSize} out of {totalCount} records fetched" : @"failed",
                    total = totalCount,
                    data = assigned.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                };
            }
            catch (Exception x)
            {
                return response = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> AssignVehicleToDriverAsync(DriverVehicleRecord payLoad)
        {
            //TODO: assigns a driver to a vehicle
            DefaultAPIResponse rsp = null;

            try
            {
                Helper helper = new Helper();

                int driver_ID = await helper.getDriverIDAsync(payLoad.driveremail);
                var oVehicle = await new clsVehicle() { }.Get(payLoad.vehicleRegistration);

                bool bln = await new Helper() { }.AssignDriverAsync(payLoad.driveremail, driver_ID, oVehicle);

                return rsp = new DefaultAPIResponse() { 
                    status = bln,
                    message = bln ? $"Driver {payLoad.driveremail} assigned to vehicle {payLoad.vehicleRegistration} successfully!!": @"failed",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
