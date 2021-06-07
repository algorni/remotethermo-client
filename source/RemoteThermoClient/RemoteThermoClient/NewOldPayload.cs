using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class NewOldPayload<numericType>
    {
        public NewOldPayload()
        { 
        
        }

        public NewOldPayload(numericType @new, numericType old)
        {
            this.@new = @new;
            this.old = old;
        }

        public numericType @new { get; set; }
        public numericType old { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
