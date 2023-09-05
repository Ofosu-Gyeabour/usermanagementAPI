using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ILoggerService
    {
        Task<DefaultAPIResponse> WriteLogAsync(Log oLogger);
        Task<DefaultAPIResponse> GetLogsAsync();
        Task<DefaultAPIResponse> GetLogsAsync(SingleParam _user);
    }
}
