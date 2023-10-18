#nullable disable
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Models;

namespace UserManagementAPI.utils
{
    public class Helper
    {
        swContext config;
        public Helper() {
            config = new swContext();
        }   

        public async Task<bool> AmendUserModules(string _usr, string _newProfile)
        {
            try
            {    
                try
                {
                    var pObj = await config.TProfiles.Where(p => p.ProfileName == _newProfile).FirstOrDefaultAsync();
                    var u = await config.Tusrs.Where(x => x.Usrname == _usr).FirstOrDefaultAsync();
                    if ((u != null) & (pObj != null))
                    {
                        //amend associated profile id
                        u.ProfileId = pObj.ProfileId;
                    }

                    await config.SaveChangesAsync();

                    return true;
                }
                catch (Exception transErr)
                {
                    throw;
                }   
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    
        public async Task<int> getCountryIdAsync(string name)
        {
            //gets the country Id
            try
            {
                var obj = await config.TCountryLookups.Where(x => x.CountryName == name).FirstOrDefaultAsync();
                return obj.CountryId;
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return 0;
            }
        }

        public async Task<TCountryLookup> getCountryAsync(int id)
        {
            //gets the country object from the data store
            TCountryLookup _country;

            try
            {
                _country = await config.TCountryLookups.Where(x => x.CountryId == id).FirstOrDefaultAsync();
                return _country;
            }
            catch(Exception x)
            {
                return _country = new TCountryLookup() { CountryId = 0 };
            }
        }

        public async Task<CountryLookup> getCountry(int Id)
        {
            CountryLookup obj = null;
            try
            {
                var dt = await config.TCountryLookups.Where(x => x.CountryId == Id).Include(r => r.Region).FirstOrDefaultAsync();
                if (dt != null)
                {
                    obj = new CountryLookup()
                    {
                        id = Id,
                        nameOfcountry = dt.CountryName,
                        codeOfcountry = dt.CountryCode,
                        oRegion = new RegionLookup()
                        {
                            id = dt.Region.RegionId,
                            nameOfregion = dt.Region.RegionName
                        }
                    };
                }

                return obj;
            }
            catch(Exception x)
            {
                return obj = new CountryLookup() { id = 0 };
            }
        }
        public async Task<TCountryLookup> getCountryAsync(string name)
        {
            //gets the country object from the data store
            TCountryLookup _country;

            try
            {
                _country = await config.TCountryLookups.Where(x => x.CountryName == name).FirstOrDefaultAsync();
                return _country;
            }
            catch (Exception x)
            {
                return _country = new TCountryLookup() { CountryId = 0 };
            }
        }

        public async Task<TCity> getCityAsync(string name)
        {
            //gets city record using city name
            TCity obj = null;

            try
            {
                obj = await config.TCities.Where(x => x.CityName == name).FirstOrDefaultAsync();
                return obj;
            }
            catch(Exception ex)
            {
                return obj = new TCity() { Id = 0 };
            }
        }


        //writes log
        public async Task<bool> WriteLogAsync(Log oLogger)
        {          
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

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<TEvent> getEventLookupAsync(string _evtDescription)
        {
            //gets the event object using the Id
            TEvent objEvent = null;
            try
            {
                var obj = await config.TEvents.Where(x => x.EventDescription == _evtDescription).FirstOrDefaultAsync();
                if (obj.Id > 0)
                {
                    objEvent = obj;
                }

                return objEvent;
            }
            catch(Exception x)
            {
                return objEvent;
            }
        }

    }
}
