using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class GetPlantDataResponse
    {
        public string gw { get; set; }
        public bool on { get; set; }
        public PlantModeEnum mode { get; set; }
        public double waterTemp { get; set; }
        public double comfortTemp { get; set; }
        public double reducedTemp { get; set; }
        public double procReqTemp { get; set; }
        public OperativeModeEnum opMode { get; set; }
        public DateTime holidayUntil { get; set; }
        public bool boostOn { get; set; }
        public hpStateEnum hpState { get; set; }


        public static GetPlantDataResponse FromJson(string jsonString)
        {
            GetPlantDataResponse loginResponse = JsonConvert.DeserializeObject<GetPlantDataResponse>(jsonString);

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
