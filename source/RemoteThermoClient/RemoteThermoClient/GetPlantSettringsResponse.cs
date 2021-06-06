using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class GetPlantSettringsResponse
    {
        public double SlpMaxGreenTemperature { get; set; }
        public double SlpMaxSetpointTemperature { get; set; }
        public double SlpMaxSetpointTemperatureMin { get; set; }
        public double SlpMaxSetpointTemperatureMax { get; set; }
        public double SlpMinSetpointTemperature { get; set; }
        public double SlpMinSetpointTemperatureMin { get; set; }
        public double SlpMinSetpointTemperatureMax { get; set; }
        public double SlpAntilegionellaOnOff { get; set; }
        public double SlpPreHeatingOnOff { get; set; }
        public double SlpHeatingRate { get; set; }
        public double SlpHcHpMode { get; set; }

        public static GetPlantSettringsResponse FromJson(string jsonString)
        {
            GetPlantSettringsResponse loginResponse = JsonConvert.DeserializeObject<GetPlantSettringsResponse>(jsonString);

            return loginResponse;
        }

        /// <summary>
        /// Objtain the JSON Reppresentation
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
