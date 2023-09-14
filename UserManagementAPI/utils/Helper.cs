#nullable disable
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

        

    }
}
