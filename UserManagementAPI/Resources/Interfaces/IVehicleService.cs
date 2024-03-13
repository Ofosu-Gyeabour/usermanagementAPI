#nullable disable
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IVehicleService
    {
        Task<DefaultAPIResponse> SaveVehicleAsync(clsVehicle payLoad);

        Task<PaginationAPIResponse> ListVehiclesAsync(int page, int pageSize);

        Task<PaginationAPIResponse> ListUnassignedVehiclesAsync(int page, int pageSize);

        Task<DefaultAPIResponse> GetUnassignedVehiclesAsync();
        Task<PaginationAPIResponse> GetUnassignedDriversAsync(List<userRecord> userRecords, int page, int pageSize);
        Task<DefaultAPIResponse> GetALLDriversUnassigned(List<userRecord> userRecords);

        Task<PaginationAPIResponse> GetAssignedDriversAsync(int page, int pageSize);

        Task<DefaultAPIResponse> AssignVehicleToDriverAsync(DriverVehicleRecord payLoad);
    }
}
