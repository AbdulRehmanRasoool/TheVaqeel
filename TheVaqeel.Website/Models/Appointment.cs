using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheVaqeel.Website.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }   
        public DateTime Date { get; set; }
        public int TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string LawyerDetailId { get; set; }
        public LawyerDetail LawyerDetail { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int Status { get; set; }
    }
}