using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteThermoClient
{
    public class NewOldPayload<numericType>
    {
        public numericType @new { get; set; }
        public numericType old { get; set; }
    }
}
