using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Famly.Model
{
    public class AuthenticateRequest
    {
        public string email { get; set; }
        public string password { get; set; }
        public string deviceId { get; set; } = Guid.NewGuid().ToString();
        public string locale { get; set; } = new CultureInfo("da-DK").ToString();
    }
}
