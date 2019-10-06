using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Famly.Model
{
    public class AuthenticateResponse
    {
        public string accessToken { get; set; }
        public string deviceId { get; set; }
        public Links links { get; set; }

        public class Links
        {
            public string me { get; set; }
        }
    }
}
