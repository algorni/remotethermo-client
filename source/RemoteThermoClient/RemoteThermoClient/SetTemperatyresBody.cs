using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class SetTemperatyresBody
    {
        public SetTemperatyresBody(double comfort, double reduced)
        {
            @new = new New() { comfort = comfort, reduced = reduced };  
            
            //the "old" just must be different to work! 
            old = new Old() { comfort = comfort+1.0, reduced = reduced+1.0 };
        }

        public New @new { get; set; }
        public Old old { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
    public class New
    {
        public double comfort { get; set; }
        public double reduced { get; set; }
    }

    public class Old
    {
        public double comfort { get; set; }
        public double reduced { get; set; }
    }
}
