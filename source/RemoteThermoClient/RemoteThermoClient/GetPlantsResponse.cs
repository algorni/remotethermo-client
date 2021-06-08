using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class GetPlantsResponse
    {
        public List<Plant> Plants { get; set; }

        public static GetPlantsResponse FromJson(string jsonString)
        {
            GetPlantsResponse getPlantsResponse = new GetPlantsResponse();

            getPlantsResponse.Plants = JsonConvert.DeserializeObject<List<Plant>>(jsonString);

            return getPlantsResponse;
        }

        /// <summary>
        /// Objtain the JSON Reppresentation
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this.Plants, Formatting.Indented);
        }
    }

    public class Plant
    { 
        public bool notifyOnErrors { get; set; }
        public bool notifyOnReadyShowers { get; set; }
        public bool notifyOnCondensateTankFull { get; set; }
        public bool hpmpSys { get; set; }
        public int wheType { get; set; }
        public string gw { get; set; }
        public string sn { get; set; }
        public string name { get; set; }
        public Loc loc { get; set; }
        public int sys { get; set; }
        public int utcOft { get; set; }
        public int lnk { get; set; }
        public int weatherProvider { get; set; }
        public bool tcByGuest { get; set; }
        public ConsumptionsSettings consumptionsSettings { get; set; }
        public GeofenceConfig geofenceConfig { get; set; }
        public int mqttApiVersion { get; set; }
        public bool isOffline48H { get; set; }
    }

    public class Loc
    {
        public string country { get; set; }
        public string addr { get; set; }
        public string cityName { get; set; }
        public int city { get; set; }
        public string adm1Name { get; set; }
        public int adm1 { get; set; }
        public string adm2Name { get; set; }
        public int adm2 { get; set; }
        public string postalCode { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public double radius { get; set; }
    }

    public class ConsumptionsSettings
    {
        public int currency { get; set; }
        public int gasType { get; set; }
        public int gasEnergyUnit { get; set; }
    }

    public class GeofenceConfig
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public double radius { get; set; }
    }

}
