using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
        private string _plantName;
        private string _plantId;
        
        private LoginResponse _loginResponse = null;

        private GetPlantsResponse _plantResponse = null;


        private const string url_Login = "/api/v2/accounts/login";

        private const string url_Plants = "/api/v2/velis/plants";

        private const string url_PlantData = "/api/v2/velis/slpPlantData/";

        private const string url_time = "/api/v2/remote/timeProgs/";

        



        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="plantId"></param>
        /// <param name="baseUrlComponent"></param>
        /// <param name="logger"></param>
        public RemoteThermoClient(string userName, string password, string plantName, string baseUrlComponent, ILogger<RemoteThermoClient> logger)
        {
            _userName = userName;
            _password = password;
            _plantName = plantName;

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

        private async Task ensureLoginAndGetPlantId(bool justLogin = false)
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

            if (!justLogin)
            {
                _plantResponse = await GetPlants(true);

                _plantId = (from plant in _plantResponse.Plants
                            where plant.name == _plantName
                            select plant.gw).FirstOrDefault();

                if (string.IsNullOrEmpty(_plantId))
                {
                    throw new ApplicationException($"Impossible to find Plant By Name: {_plantName}");
                }
            }
        }

        public async Task<GetPlantsResponse> GetPlants(bool reload = false)
        {
            if (_plantResponse != null && !reload)
                return _plantResponse;

            await ensureLoginAndGetPlantId(true);

            var getPlantsUrl = _baseUrl + url_Plants;

            var response = await httpClient.GetAsync(getPlantsUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                GetPlantsResponse getPlantsResponse = GetPlantsResponse.FromJson(responseContentString);

                return getPlantsResponse;
            }
            else
            {
                _logger.LogError($"Error while retreiving Plants. Http Status Code: {response.StatusCode}");
                return null;
            }
        }


        public async Task<GetPlantDataResponse> GetPlantData()
        {
            await ensureLoginAndGetPlantId();
                       
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
            await ensureLoginAndGetPlantId();

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


        public async Task SwitchOnOff(bool status)
        {
            await ensureLoginAndGetPlantId();

            var postSwitchStatus = _baseUrl + url_PlantData + _plantId + "/switch";

            var switchOnOffContent = new StringContent(status.ToString(), Encoding.UTF8, "application/json");

            var response = await httpClient.GetAsync(postSwitchStatus);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Heater switched: {status}");
            }
            else
            {
                var message = $"Error while switching status. Http Status Code: {response.StatusCode}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task UpdatePlantMode(PlantModeEnum plantMode)
        {
            await ensureLoginAndGetPlantId();

            var postUpdatePlantMode = _baseUrl + url_PlantData + _plantId + "/plantMode";

            var jsonBody = new NewOldPayload<int>((int)plantMode, ((int)plantMode + 1)).ToJson();

            var switchOnOffContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.GetAsync(postUpdatePlantMode);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated Plant Mode to: {plantMode}");
            }
            else
            {
                var message = $"Error while updating Plant Mode. Http Status Code: {response.StatusCode}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task UpdateOperativeMode(OperativeModeEnum operativeMode)
        {
            await ensureLoginAndGetPlantId();

            var postUpdateOperativeMode = _baseUrl + url_PlantData + _plantId + "/operativeMode";

            var jsonBody = new NewOldPayload<int>((int)operativeMode, ((int)operativeMode + 1)).ToJson();

            var switchOnOffContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.GetAsync(postUpdateOperativeMode);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated Operative Mode to: {operativeMode}");
            }
            else
            {
                var message = $"Error while updating Operative Mode. Http Status Code: {response.StatusCode}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task SetBoostOnOff(bool status)
        {
            await ensureLoginAndGetPlantId();

            var postSwitchStatus = _baseUrl + url_PlantData + _plantId + "/boost";

            var switchOnOffContent = new StringContent(status.ToString(), Encoding.UTF8, "application/json");

            var response = await httpClient.GetAsync(postSwitchStatus);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Heater Boost switched: {status}");
            }
            else
            {
                var message = $"Error while switching Boost status. Http Status Code: {response.StatusCode}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task SetTemperature(double comfort, double reduced)
        {
            var setTemperatureUrl = _baseUrl + url_PlantData + _plantId + "/temperatures";

            var setTemperatureContent = new StringContent(new SetTemperatyresBody(comfort,reduced).ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(setTemperatureUrl, setTemperatureContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated Temperatures: Comfort {comfort} Reduced {reduced}");
            }
            else
            {
                var message = $"Error while setting Temperatures: Comfort {comfort} Reduced {reduced}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task SetHolidayUntil(DateTime? holiday)
        {
            var setHolidayUrl = _baseUrl + url_PlantData + _plantId + "/holiday";

            var setHolidayContent = new StringContent(new SetHolidayBody(holiday).ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(setHolidayUrl, setHolidayContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated Holiday: {holiday}");
            }
            else
            {
                var message = $"Error while updating Holiday {holiday}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task SetAntiLegionella(bool status)
        {
            var setPlantSettingsUrl = _baseUrl + url_PlantData + _plantId + "/plantSettings";

            var setPlantSettingsContent = new StringContent(new SetAntiLegionellaBody(status).ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(setPlantSettingsUrl, setPlantSettingsContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated Antilegionella: {status}");
            }
            else
            {
                var message = $"Error while updating Antilegionella {status}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }

        public async Task SetPreHeating(bool status)
        {
            var setPlantSettingsUrl = _baseUrl + url_PlantData + _plantId + "/plantSettings";

            var setPlantSettingsContent = new StringContent(new SetPreHeatingBody(status).ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(setPlantSettingsUrl, setPlantSettingsContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated PreHeating: {status}");
            }
            else
            {
                var message = $"Error while updating PreHeating {status}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }

        public async Task SetMaxSetpointTemperature(double maxSetpointTemperature)
        {
            var setPlantSettingsUrl = _baseUrl + url_PlantData + _plantId + "/plantSettings";

            var setPlantSettingsContent = new StringContent(new SetMaxSetpointTemperatyreBody(maxSetpointTemperature).ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(setPlantSettingsUrl, setPlantSettingsContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated MaxSetpointTemperature: {maxSetpointTemperature}");
            }
            else
            {
                var message = $"Error while updating MaxSetpointTemperature {maxSetpointTemperature}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }


        public async Task<ScheduleBody> GetSchedule()
        {
            await ensureLoginAndGetPlantId();

            var scheduleUrl = _baseUrl + url_time + _plantId + "/Dhw";

            var response = await httpClient.GetAsync(scheduleUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();

                ScheduleBody getPlantSettringsResponse = ScheduleBody.FromJson(responseContentString);

                return getPlantSettringsResponse;
            }
            else
            {
                _logger.LogError($"Error while retreiving Schedule. Http Status Code: {response.StatusCode}");
                return null;
            }
        }


        public async Task SetSchedule(ScheduleBody schedule)
        {
            var scheduleUrl = _baseUrl + url_time + _plantId + "/Dhw";

            var setPlantSettingsContent = new StringContent(schedule.ToJson(), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(scheduleUrl, setPlantSettingsContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated MaxSetpointTemperature: {schedule}");
            }
            else
            {
                var message = $"Error while updating MaxSetpointTemperature {schedule}";

                _logger.LogError(message);

                throw new ApplicationException(message);
            }
        }

    }
}
