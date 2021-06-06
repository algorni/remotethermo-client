using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class LoginResponse
    {
        public string token { get; set; }
        public Act act { get; set; }

        /// <summary>
        /// Static constructor from JSON 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static LoginResponse FromJson(string jsonString)
        {
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonString);

            return loginResponse;
        }
    }

    public class CountryBounds
    {
        public string country { get; set; }
        public double neLat { get; set; }
        public double neLng { get; set; }
        public double swLat { get; set; }
        public double swLng { get; set; }
        public double markerLat { get; set; }
        public double markerLng { get; set; }
    }

    public class Act
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string CountryCode { get; set; }
        public bool AcceptTermsAndCondsOfGdpr { get; set; }
        public CountryBounds CountryBounds { get; set; }
    }
}
