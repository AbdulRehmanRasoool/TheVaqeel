using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheVaqeel.Website.Models;
using TheVaqeel.Website.ViewModel;

namespace TheVaqeel.Website.Controllers
{
    public class AppointmentController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Create(string id)
        {
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            var timeSlots = context.TimeSlots.Where(t => t.IsAvailaible == false).ToList();
            var lawyer = context.LawyerDetails.Include(l => l.LawyerPracticeAreas).SingleOrDefault(l => l.LawyerId == id);
            var userId = User.Identity.GetUserId();
            var user = context.Users.SingleOrDefault(u => u.Id == userId);
            homeViewModel.HomeNavbar = navbarElements;
            homeViewModel.LawyerDetail = lawyer;
            homeViewModel.ApplicationUser = user;
            homeViewModel.TimeSlots = timeSlots;    
            return View(homeViewModel);
        }
        [HttpPost]
        public ActionResult Create(HomeViewModel homeViewModel)
        {
            var userId = User.Identity.GetUserId();
            homeViewModel.Appointment.UserId = userId;
            homeViewModel.Appointment.LawyerDetailId = homeViewModel.LawyerDetail.LawyerId;
            homeViewModel.Appointment.Status = 0;
            context.Appointments.Add(homeViewModel.Appointment);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}