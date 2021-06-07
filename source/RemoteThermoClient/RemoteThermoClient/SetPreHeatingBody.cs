using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class SetPreHeatingBody
    {
        public SetPreHeatingBody(bool status)
        {
            if (status)
            {
                SlpPreHeatingOnOff = new NewOldPayload<double>() { @new = 1.0, old = 0.0 };  

            }
            else
            {
                SlpPreHeatingOnOff = new NewOldPayload<double>() { @new = 0.0, old = 1.0 };  
            }
        }

        public NewOldPayload<double> SlpPreHeatingOnOff { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
