﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheVaqeel.Website.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}