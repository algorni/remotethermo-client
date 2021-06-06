using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class LoginRequestBody
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LoginRequestBody()
        {
            imp = false;
            notTrack = true;
        }

        public LoginRequestBody(string userName, string password)
        {
            imp = false;
            notTrack = true;

            usr = userName;
            pwd = password;
        }

        public string usr { get; set; }
        public string pwd { get; set; }
        public bool imp { get; set; }
        public bool notTrack { get; set; }

        /// <summary>
        /// Objtain the JSON Reppresentation
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented);
        }
    }
}
