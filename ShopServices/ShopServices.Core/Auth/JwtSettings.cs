using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Auth
{
    public class JwtSettings
    {
        public string Issuer {  get; set; }
        public string Audience { get; set; }
        public string KEY { get; set; }
    }
}
