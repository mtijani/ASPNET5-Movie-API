﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieAPI.DTOs
{
    public class AutheticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
