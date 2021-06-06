using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RemoteThermoClient
{
    public class RemoteThermoClient
    {
        
        private static HttpClient httpClient = new HttpClient();

        private ILogger _logger;



        //the overall url for the API will be something like https://www.<baseUrlComponent>.remotethermo.com/api/v2/<api-specific>
        private string _baseUrl = null;
        private string _userName;
        private string _password;
        private string _plantId;

        private string _token;

        private LoginResponse _loginResponse = null;

        

        private const string url_Login = "/api/v2/accounts/login";
        private const string url_PlantData = "/api/v2/velis/slpPlantData/";


        



        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="plantId"></param>
        /// <param name="baseUrlComponent"></param>
        /// <param name="logger"></param>
        public RemoteThermoClient(string userName, string password, string plantId, string baseUrlComponent, ILogger<RemoteThermoClient> logger)
        {
            _userName = userName;
            _password = password;
            _plantId = plantId;

            _baseUrl = $"https://www.{baseUrlComponent}.remotethermo.com";

            _logger = logger;
        }


        public async Task<LoginResponse> DoLogin()
        {
            var loginUrl = _baseUrl + url_Login;

            var loginRequestContent = new StringContent( new LoginRequestBody(_userName,_password).ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(loginUrl, loginRequestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                LoginResponse loginResponse = LoginResponse.FromJson(responseContentString);

                return loginResponse;
            }
            else
            {
                _logger.LogError($"Login error wrong user name and password: {_userName} {_password}");
                return null;
            }            
        }

        private async Task ensureLogin()
        {
            if (_loginResponse == null)
            {
                _loginResponse = await DoLogin();

                if (_loginResponse != null)
                {
                    httpClient.DefaultRequestHeaders.Add("ar.authtoken", _loginResponse.token);
                }
                else
                {
                    throw new ApplicationException($"Login error!");
                }                
            }
        }

        public async Task<GetPlantDataResponse> GetPlantData()
        {
            await ensureLogin();
                       
            var getPlantDataUrl = _baseUrl + url_PlantData + _plantId;

            var response = await httpClient.GetAsync(getPlantDataUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                GetPlantDataResponse getPlantDataResponse = GetPlantDataResponse.FromJson(responseContentString);

                return getPlantDataResponse;
            }
            else
            {
                _logger.LogError($"Error while retreiving Plant Data. Http Status Code: {response.StatusCode}");
                return null;
            }
        }



        public async Task<GetPlantSettringsResponse> GetPlantSettrings()
        {
            await ensureLogin();

            var getPlantDataUrl = _baseUrl + url_PlantData + _plantId + "/plantSettings";

            var response = await httpClient.GetAsync(getPlantDataUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                GetPlantSettringsResponse getPlantSettringsResponse = GetPlantSettringsResponse.FromJson(responseContentString);

                return getPlantSettringsResponse;
            }
            else
            {
                _logger.LogError($"Error while retreiving Plant Settings. Http Status Code: {response.StatusCode}");
                return null;
            }
        }

    }
}
