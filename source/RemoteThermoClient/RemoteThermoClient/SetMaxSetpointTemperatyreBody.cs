using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class SetMaxSetpointTemperatyreBody
    {
        public SetMaxSetpointTemperatyreBody(double temperature)
        {
            SlpMaxSetpointTemperature = new NewOldPayload<double>() { @new = temperature, old = temperature - 1.0 };  //the "old" just must be different to work! 
        }


        public NewOldPayload<double> SlpMaxSetpointTemperature { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
