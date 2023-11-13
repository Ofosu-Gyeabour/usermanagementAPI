#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Diagnostics;
using UserManagementAPI.utils;
using System.Net;
using System.Net.Security;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace UserManagementAPI.Resources.Implementations
{
    public class PostCodeService : IPostCodeService
    {
        public swContext config;
        public PostCodeService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetAddressesAsync(SingleParam payLoad)
        {
            //gets the list of addresses for a post code
            DefaultAPIResponse response = null;
            PostCodeAddressItems addresses = null;

            try
            {
                var client = await BuildHTTPClient(string.Format("{0}Key={1}&UserName={2}&SearchTerm={3}", PostCodeConfigObject.FIND_POST_CODE, 
                                                                    PostCodeConfigObject.KEY, 
                                                                    PostCodeConfigObject.USER,
                                                                    payLoad.stringValue));


                var rsp = await client.GetAsync(client.BaseAddress);
                rsp.EnsureSuccessStatusCode();
                if (rsp.IsSuccessStatusCode)
                {
                    var responseBody = await rsp.Content.ReadAsStringAsync();
                    addresses = JsonConvert.DeserializeObject<PostCodeAddressItems>(responseBody);
                }

                //return object
                response = new DefaultAPIResponse();
                if (addresses.Items.Count() > 0)
                {
                    response.status = true;
                    response.message = @"success";
                    response.data = addresses;
                }
                else
                {
                    response.status = false;
                    response.message = @"No data";
                    response.data = addresses;
                }

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

        private async Task<HttpClient> BuildHTTPClient(string URL)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            clientHandler.UseProxy = false;

            var client = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(URL)
            };

            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(_contentType));
            client.DefaultRequestHeaders.Add("accept", "*/*");


            return client;
        }

    }
}
