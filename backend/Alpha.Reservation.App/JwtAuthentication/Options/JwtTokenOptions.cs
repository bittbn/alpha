﻿using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Alpha.Reservation.App.JwtAuthentication.Options
{
    public class JwtTokenOptions
    {
        public string SecurityKey { get; set; }

        public string Issuer { get; set; }

        public TimeSpan LifeTime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey));

        public DateTime GetExpires() => DateTime.Now.AddDays(LifeTime.Days);
    }
}