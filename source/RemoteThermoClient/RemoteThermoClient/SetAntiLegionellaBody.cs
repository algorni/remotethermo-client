using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class SetAntiLegionellaBody
    {
        public SetAntiLegionellaBody(bool status)
        {
            if (status)
            {
                SlpAntilegionellaOnOff = new NewOldPayload<double>() { @new = 1.0, old = 0.0 };  

            }
            else
            {
                SlpAntilegionellaOnOff = new NewOldPayload<double>() { @new = 0.0, old = 1.0 };  
            }
        }

        public NewOldPayload<double> SlpAntilegionellaOnOff { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
