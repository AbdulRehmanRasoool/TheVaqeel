using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using TheVaqeel.Website.Models;
using TheVaqeel.Website.ViewModel;

namespace TheVaqeel.Website.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();


        [HttpGet]
        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            var homeSliders = context.HomeSliders.ToList();
            var lawyerServices = context.LawyerServiceContents.ToList();
            homeViewModel.HomeNavbar = navbarElements;
            homeViewModel.HomeSlider = homeSliders;
            homeViewModel.LawyerServiceContent = lawyerServices;
            var userId = User.Identity.GetUserId();
            var lawyer = context.LawyerDetails.Any(l => l.LawyerId == userId);
            var lawyers = context.LawyerDetails.Include(l => l.LawyerPracticeAreas).ToList();
            homeViewModel.LawyerDetails = lawyers;
            var isValid = false;
            if (lawyer == true)
            {
                isValid = true;
            }
            ViewBag.IsValid = isValid;
            return View(homeViewModel);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            homeViewModel.HomeNavbar = navbarElements;
            return View(homeViewModel);
        }

        [HttpPost]
        public ActionResult Contact(ContactUs contactUs)
        {
            context.ContactUs.Add(contactUs);
            context.SaveChanges();
            return RedirectToAction("Contact","Home");
        }

        [HttpGet]
        public ActionResult FAQs()
        {
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            var faqs = context.Faqs.ToList();
            homeViewModel.HomeNavbar = navbarElements;
            homeViewModel.Faq = faqs;
            return View(homeViewModel);
        }

        [HttpGet]
        public ActionResult AttorneyDetail(string id)
        {
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            homeViewModel.HomeNavbar = navbarElements;
            var lawyer = context.LawyerDetails.Include(l =>l.LawyerPracticeAreas).Include(l => l.LawyerLocation).SingleOrDefault(l => l.LawyerId == id);
            homeViewModel.LawyerDetail = lawyer;
            return View(homeViewModel);
        }

        [HttpGet]
        public ActionResult AttorneyList()
        {

            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            homeViewModel.HomeNavbar = navbarElements;
            var lawyers = context.LawyerDetails.Include(l => l.LawyerPracticeAreas).ToList();
            homeViewModel.LawyerDetails = lawyers;
            return View(homeViewModel);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult UserAppointment()
        {
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            homeViewModel.HomeNavbar = navbarElements;
            var userId = User.Identity.GetUserId();
            var appointments = context.Appointments.Include(a => a.TimeSlot).Include(a => a.LawyerDetail).Where(a => a.UserId == userId).ToList();
            if(appointments.Count == 0)
            {
                ViewBag.Msg = "You have no appointments";
            }
            homeViewModel.Appointments = appointments;  
            return View(homeViewModel);
        }
    }
}