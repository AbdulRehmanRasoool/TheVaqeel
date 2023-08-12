using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheVaqeel.Website.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailaible { get; set; }
    }
}