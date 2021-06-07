using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class ScheduleBody
    {
        public Dhw Dhw { get; set; }

        public static ScheduleBody FromJson(string jsonString)
        {
            ScheduleBody loginResponse = JsonConvert.DeserializeObject<ScheduleBody>(jsonString);

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

    public class Slice
    {
        public int from { get; set; }
        public int temp { get; set; }
    }

    public class Plan
    {
        public List<int> days { get; set; }
        public List<Slice> slices { get; set; }
    }

    public class Dhw
    {
        public List<Plan> plans { get; set; }
        public List<int> allowedTemp { get; set; }
        public int defaultTemp { get; set; }
        public double baseTemp { get; set; }
        public double tick { get; set; }
        public int maxSwitches { get; set; }
        public bool ext { get; set; }
    }
}
