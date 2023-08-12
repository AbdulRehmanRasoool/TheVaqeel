using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using TheVaqeel.Website.Models;

namespace TheVaqeel.Website.ViewModel
{
    public class HomeViewModel
    {
        public HomeNavbar HomeNavbar { get; set; }
        public List<HomeSlider> HomeSlider { get; set; }
        public List<Faqs> Faq { get; set; }
        public List<LawyerServiceContent> LawyerServiceContent { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public SetPasswordViewModel SetPasswordViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public LawyerDetail LawyerDetail { get; set; }
        public List<LawyerLocation> LawyerLocation { get; set; }
        public List<LawyerPracticeAreas> LawyerPracticeAreas { get; set; }
        public List<LawyerDetail> LawyerDetails { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
        public Appointment Appointment { get; set; }
        public List<Appointment> Appointments { get; set; }
        public IQueryable<Appointment> Appointmentts { get; set; }
    }
}