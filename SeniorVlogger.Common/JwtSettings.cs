﻿namespace SeniorVlogger.Common
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public double ExpirationInMinutes { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
