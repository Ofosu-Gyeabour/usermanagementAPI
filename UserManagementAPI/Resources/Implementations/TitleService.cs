#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Runtime.InteropServices;

namespace UserManagementAPI.Resources.Implementations
{
    public class TitleService : ITitleService
    {
        swContext config;
        public TitleService()
        {
            config = new swContext();
        }
        public async Task<DefaultAPIResponse> GetTitlesAsync()
        {
            DefaultAPIResponse response = null;
            List<TitleLookup> titles = null;

            try
            {
                var dta = await config.TTitles.ToListAsync();
                if (dta != null)
                {
                    titles = new List<TitleLookup>();
                    foreach(var d in dta)
                    {
                        var obj = new TitleLookup() { 
                            id = d.Id,
                            nameOftitle = d.Title
                        };

                        titles.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = titles
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> CreateTitleAsync(TitleLookup payLoad)
        {
            DefaultAPIResponse response = null;

            try
            {
                var dt = await config.TTitles.Where(t => t.Title == payLoad.nameOftitle).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TTitle objTitle = new TTitle()
                    {
                        Title = payLoad.nameOftitle
                    };

                    await config.AddAsync(objTitle);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Title {payLoad.nameOftitle} added to data store",
                        data = payLoad
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"Title '{payLoad.nameOftitle}' already exist in the data store"}; }

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
