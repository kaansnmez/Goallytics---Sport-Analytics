using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Tools
{
    public class JwtTokenDefaults
    {
        public const string ValidIssuer = "http://localhost";
        public const string ValidAudience = "http://localhost";
        public const string IssuerSigningKey = "this_is_a_long_secret_key_1234567890_!";
        public const int ExpireMinutes = 60;
    }
}
