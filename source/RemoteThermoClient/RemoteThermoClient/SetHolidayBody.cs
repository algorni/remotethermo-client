using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class SetHolidayBody
    {
        public SetHolidayBody(DateTime? holidayUntil)
        {
            @new = holidayUntil;

            if (holidayUntil.HasValue)
            {
                old = (DateTime?)null;
            }
            else
            {
                //need to set to a fictitus time just to be different.
                old = DateTime.MinValue;
            }
        }

        public DateTime? @new { get; set; }
        public DateTime? old { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
