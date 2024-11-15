﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}
