﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Domain.DTOs
{
    public class ConfigurationSMPT
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public bool IsTest  { get; set; }
        public string EmailTest { get; set; }
        public string PathEmail { get; set; }
        public bool Activated { get; set; }
    }
}
