using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Implementations
{
    public class LoggerService : ILoggerService
    {
        swContext config;

        public LoggerService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> WriteLogAsync(Log oLogger)
        {
            DefaultAPIResponse response = null;

            try
            {
                var obj = new TLogger()
                {
                    LogId = oLogger.id,
                    LogEvent = oLogger.eventId,
                    LogActor = oLogger.actor,
                    LogEntity = oLogger.entity,
                    LogEntityValue = oLogger.entityValue,
                    CompanyId = oLogger.companyId,
                    LogDate = oLogger.logDate
                };

                await config.TLoggers.AddAsync(obj);
                await config.SaveChangesAsync();

                response = new DefaultAPIResponse() { 
                   status = true, 
                   message = $"event logged for user {oLogger.actor}",
                   data = (object) oLogger
                };

                return response;
            }
            catch(Exception ex)
            {
                var x = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"{ex.Message} | {ex.InnerException.Message}"
                };

                return x;
            }
        }

        public async Task<DefaultAPIResponse> GetLogsAsync()
        {
            //method gets all the logs in the data store
            DefaultAPIResponse response = null;
            List<Log> logData = new List<Log>();

            try
            {
                var log_list = await config.TLoggers.ToListAsync();

                if (log_list.Count() > 0)
                {
                    foreach(var d in log_list)
                    {
                        var obj = new Log()
                        {
                            id = d.LogId,
                            eventId = d.LogEvent,
                            actor = d.LogActor,
                            entity = d.LogEntity,
                            entityValue = d.LogEntityValue,
                            companyId = d.CompanyId,
                            logDate = d.LogDate
                        };

                        logData.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = (object)logData
                    };
                }
                else
                {
                    response = new DefaultAPIResponse() { status = false, message = @"No data" };
                }
               
                return response;
            }
            catch(Exception x)
            {
                var errorObj = new DefaultAPIResponse() { status = false, message = $"{x.Message}"};
                return errorObj;
            }
        }

        public async Task<DefaultAPIResponse> GetLogsAsync(SingleParam _user)
        {
            //method gets the log entries for a particular user
            DefaultAPIResponse response = null;
            List<Log> _logList = new List<Log>();

            try
            {
                var log_data = await config.TLoggers.Where(x => x.LogActor == _user.stringValue).ToListAsync();

                if (log_data.Count() > 0)
                {
                    foreach(var d in log_data)
                    {
                        var o = new Log()
                        {
                            id = d.LogId,
                            eventId = d.LogEvent,
                            actor = d.LogActor,
                            entity = d.LogEntity,
                            entityValue = d.LogEntityValue,
                            companyId = d.CompanyId,
                            logDate = d.LogDate
                        };

                        _logList.Add(o);
                    }

                    response = new DefaultAPIResponse()
                    {
                       status = true,
                       message = $"successful retrieval of {_logList.Count().ToString()} logged record(s)",
                       data = (object)_logList
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception ex)
            {
                var errorObj = new DefaultAPIResponse() { status = false, message = $"{ex.Message}" };
                return errorObj;
            }
        }

    }
}
