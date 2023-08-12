using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TheVaqeel.Website.Models;
using TheVaqeel.Website.ViewModel;

namespace TheVaqeel.Website.Controllers
{ 
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var lawyerRoleId = roleManager.Roles.Where(r => r.Name == "Lawyer").Select(r => r.Id).FirstOrDefault();
            var totalLawyers = userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == lawyerRoleId)).ToList().Count();

            var userRoleId = roleManager.Roles.Where(r => r.Name == "User").Select(r => r.Id).FirstOrDefault();
            var totalUsers = userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == userRoleId)).ToList().Count();

            var adminRoleId = roleManager.Roles.Where(r => r.Name == "Administrator").Select(r => r.Id).FirstOrDefault();
            var totalAdmins = userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == adminRoleId)).ToList().Count();

            ViewBag.TotalLawyers = totalLawyers;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalContact = context.ContactUs.Count();
            ViewBag.UserId = User.Identity.GetUserId();

            return View();
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            var contacts = context.ContactUs.ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView(contacts);
            }
            return View(contacts);
        }
        [HttpPost]
        public ActionResult DeleteContactQuery(int id)
        {
            var contact = context.ContactUs.SingleOrDefault(c => c.Id == id);
            context.ContactUs.Remove(contact);
            context.SaveChanges();
            return RedirectToAction("ContactUs", "Admin");
        }

        [HttpGet]
        public ActionResult HomeNavbar()
        {
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            Session["imgPath"] = navbarElements.Logo;
            if (Request.IsAjaxRequest())
            {
                return PartialView(navbarElements);
            }
            return View(navbarElements);
        }
        [HttpPost]
        public ActionResult HomeNavbar(HttpPostedFileBase file, HomeNavbar homeNavbar)
        {
            var navbarElementsInDb = context.HomeNavbars.SingleOrDefault(n => n.Id == homeNavbar.Id);
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var _fileName = Guid.NewGuid() + fileName;
                var extension = Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img/logo"), _fileName);

                homeNavbar.Logo = _fileName;

                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    if (file.ContentLength <= 1000000)
                    {
                        navbarElementsInDb.Email = homeNavbar.Email;
                        navbarElementsInDb.PhoneNumber = homeNavbar.PhoneNumber;
                        navbarElementsInDb.Address = homeNavbar.Address;
                        navbarElementsInDb.Location = homeNavbar.Location;
                        navbarElementsInDb.Logo = homeNavbar.Logo;
                        context.SaveChanges();
                        file.SaveAs(path);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Msg = "File Size must be Equal or less than 1mb";
                    }
                }
                else
                {
                    ViewBag.msg = "Invalid Type";
                }
            }
            else
            {
                homeNavbar.Logo = Session["imgPath"].ToString();
                navbarElementsInDb.Email = homeNavbar.Email;
                navbarElementsInDb.PhoneNumber = homeNavbar.PhoneNumber;
                navbarElementsInDb.Address = homeNavbar.Address;
                navbarElementsInDb.Location = homeNavbar.Location;
                context.SaveChanges();
                return RedirectToAction("HomeNavbar");
            }
            return RedirectToAction("HomeNavbar");
        }

        [HttpGet]
        public ActionResult HomeSlider()
        {
            var sliders = context.HomeSliders.ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView(sliders);
            }
            return View(sliders);
        }

        [HttpGet]
        public ActionResult CreateSlider()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateSlider(HttpPostedFileBase file, HomeSlider homeSlider)
        {
            var fileName = Path.GetFileName(file.FileName);
            var _fileName = Guid.NewGuid() + fileName; 
            var path = Path.Combine(Server.MapPath("~/Content/img/slider"),_fileName);
            homeSlider.Image = _fileName;
            context.HomeSliders.Add(homeSlider);
            if(context.SaveChanges() > 0)
            {
                file.SaveAs(path);
            }

            return RedirectToAction("HomeSlider", "Admin");
        }

        [HttpGet]
        public ActionResult EditSlider(int id)
        {
            var slider = context.HomeSliders.SingleOrDefault(s => s.Id == id);
            Session["imgPath"] = slider.Image;
            if (Request.IsAjaxRequest())
            {
                return PartialView(slider);
            }
            return View(slider);
        }
        [HttpPost] 
        public ActionResult EditSlider(HttpPostedFileBase file, HomeSlider homeSlider)
        {
            var sliderInDb = context.HomeSliders.SingleOrDefault(s => s.Id == homeSlider.Id);
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var _fileName = Guid.NewGuid() + fileName;
                var extension = Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img/slider"), _fileName);

                homeSlider.Image = _fileName;

                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    if (file.ContentLength <= 1000000)
                    {
                        sliderInDb.Title = homeSlider.Title;
                        sliderInDb.Description = homeSlider.Description;
                        sliderInDb.Image = homeSlider.Image;
                        context.SaveChanges();
                        file.SaveAs(path);
                        return RedirectToAction("HomeSlider");
                    }
                    else
                    {
                        ViewBag.Msg = "File Size must be Equal or less than 1mb";
                    }
                }
                else
                {
                    ViewBag.msg = "Invalid Type";
                }
            }
            else
            {
                sliderInDb.Image = Session["imgPath"].ToString();
                sliderInDb.Title = homeSlider.Title;
                sliderInDb.Description = homeSlider.Description;
                context.SaveChanges();
                return RedirectToAction("HomeSlider");
            }
            return RedirectToAction("HomeSlider");
        }

        [HttpPost]
        public ActionResult DeleteSlider(int id)
        {
            var slider = context.HomeSliders.SingleOrDefault(x => x.Id == id);
            context.HomeSliders.Remove(slider);
            context.SaveChanges();

            return RedirectToAction("HomeSlider");
        }
        [HttpGet]
        public ActionResult FaqsList()
        {
            var faqs = context.Faqs.ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView(faqs);
            }
            return View(faqs);
        }

        [HttpGet]
        public ActionResult CreateFaqs()
        {
            
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateFaqs(Faqs faqs)
        {
            context.Faqs.Add(faqs);
            context.SaveChanges();
            return RedirectToAction("FaqsList");
        }

        public ActionResult EditFaqs(int id)
        {
            var faq = context.Faqs.SingleOrDefault(f => f.Id == id);
            return PartialView(faq);
        }
        [HttpPost]
        public ActionResult EditFaqs(Faqs faqs)
        {
            var faqsInDb = context.Faqs.SingleOrDefault(f => f.Id == faqs.Id);
            faqsInDb.Question = faqs.Question;
            faqsInDb.Answer = faqs.Answer;
            context.SaveChanges();
            return RedirectToAction("FaqsList", "Admin");
        }

        [HttpPost]
        public ActionResult DeleteFaqs(int id)
        {
            var faq = context.Faqs.SingleOrDefault(f => f.Id == id);
            context.Faqs.Remove(faq);
            context.SaveChanges();

            return RedirectToAction("FaqsList", "Admin");
        }

        [HttpGet]
        public ActionResult LawyerServicesList()
        {
            var lawyerServices = context.LawyerServiceContents.ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView(lawyerServices);
            }
            return View(lawyerServices);
        }

        [HttpGet]
        public ActionResult CreateLawyerService()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateLawyerService(HttpPostedFileBase file,LawyerServiceContent lawyerServiceContent)
        {
            var fileName = Path.GetFileName(file.FileName);
            var _fileName = Guid.NewGuid() + fileName;
            var path = Path.Combine(Server.MapPath("~/Content/img/features"), _fileName);
            lawyerServiceContent.Image = _fileName;
            context.LawyerServiceContents.Add(lawyerServiceContent);
            if (context.SaveChanges() > 0)
            {
                file.SaveAs(path);
            }

            return RedirectToAction("LawyerServicesList", "Admin");
        }

        [HttpGet]
        public ActionResult EditLawyerService(int id)
        {
            var lawyerServiceInDb = context.LawyerServiceContents.SingleOrDefault(l => l.Id == id);
            Session["imgPath"] = lawyerServiceInDb.Image;
            if (Request.IsAjaxRequest())
            {
                return PartialView(lawyerServiceInDb);
            }
            return View(lawyerServiceInDb);
        }
        [HttpPost]
        public ActionResult EditLawyerService(HttpPostedFileBase file,LawyerServiceContent lawyerServiceContent)
        {
            var lawyerServiceContentInDb = context.LawyerServiceContents.SingleOrDefault(s => s.Id == lawyerServiceContent.Id);
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var _fileName = Guid.NewGuid() + fileName;
                var extension = Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img/features"), _fileName);

                lawyerServiceContent.Image = _fileName;

                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    if (file.ContentLength <= 1000000)
                    {
                        lawyerServiceContentInDb.Title = lawyerServiceContent.Title;
                        lawyerServiceContentInDb.Description = lawyerServiceContent.Description;
                        lawyerServiceContentInDb.Image = lawyerServiceContent.Image;
                        context.SaveChanges();
                        file.SaveAs(path);
                        return RedirectToAction("LawyerServicesList","Admin");
                    }
                    else
                    {
                        ViewBag.Msg = "File Size must be Equal or less than 1mb";
                    }
                }
                else
                {
                    ViewBag.msg = "Invalid Type";
                }
            }
            else
            {
                lawyerServiceContentInDb.Image = Session["imgPath"].ToString();
                lawyerServiceContentInDb.Title = lawyerServiceContent.Title;
                lawyerServiceContentInDb.Description = lawyerServiceContent.Description;
                context.SaveChanges();
                return RedirectToAction("LawyerServicesList", "Admin");
            }
            return RedirectToAction("LawyerServicesList", "Admin");
        }
        [HttpPost]
        public ActionResult DeleteLawyerService(int id)
        {
            var lawyerService = context.LawyerServiceContents.SingleOrDefault(f => f.Id == id);
            context.LawyerServiceContents.Remove(lawyerService);
            context.SaveChanges();

            return RedirectToAction("LawyerServicesList", "Admin");
        }

        [HttpGet]
        public ActionResult LawyerVerificationList()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var lawyerRoleId =roleManager.Roles.Where(r => r.Name == "Lawyer").Select(r => r.Id).FirstOrDefault();

            var lawyerUsers = userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == lawyerRoleId)).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView(lawyerUsers);
            }
            return View(lawyerUsers);
        }

        [HttpGet]
        public ActionResult LawyersList()
        {
            var lawyers = context.LawyerDetails.Include(l => l.LawyerPracticeAreas).Include(l => l.LawyerLocation).ToList();
            return View(lawyers);
        }

        [HttpGet]
        public ActionResult UsersList()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var lawyerRoleId = roleManager.Roles.Where(r => r.Name == "User").Select(r => r.Id).FirstOrDefault();

            var users = userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == lawyerRoleId)).ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult Verification(string id)
        {
            var lawyer = context.Users.SingleOrDefault(l => l.Id == id);
            if (lawyer.Status == 0)
            {
                lawyer.Status = 1;

                  string smtpServer = "smtp.gmail.com";
                string senderEmail = "thelawrexcompany@gmail.com";
                string senderPassword = "gywbbimaqvnxkggo";
                string recipientEmail = lawyer.Email;
                string subject = "Account Approved";
                string body = "Your account has been approved by the admin. Please login to add your details";

                using (var smtpClient = new SmtpClient(smtpServer, 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    // Create the email message
                    using (var mailMessage = new MailMessage(senderEmail, recipientEmail))
                    {
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = false;

                        // Send the email
                        try
                        {
                            smtpClient.Send(mailMessage);
                            ViewBag.Message = "Email sent successfully!";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "Failed to send email: " + ex.Message;
                        }
                    }
                }
            }
            else if (lawyer.Status == 1)
            {
                lawyer.Status = 0;
            }
            else
            {
                lawyer.Status = 2;
            }
            context.SaveChanges();
            return RedirectToAction("LawyerVerificationList");
        }

        [HttpGet]
        public ActionResult AdminProfile(string id)
        {
            var user = context.Users.SingleOrDefault(a => a.Id == id);
            return View(user);
        }
        [HttpPost]
        public ActionResult AdminProfile(ApplicationUser user)
        {
            var userInDb = UserManager.FindById(user.Id);
            if(userInDb != null)
            {
                userInDb.Email = user.Email;
                userInDb.UserName = user.UserName;
                var result = UserManager.Update(userInDb);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminProfile", "Admin");
                }
            }
            else
            {
                ViewBag.Error = "User Not Found";
                return View();
            }
            return RedirectToAction("AdminProfile", "Admin");
        }

        [HttpGet]
        public ActionResult PracticeAreaList()
        {
            var practiceArea = context.LawyerPracticeAreas.ToList();
            return View(practiceArea);
        }
        [HttpGet]
        public ActionResult CreatePracticeArea()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreatePracticeArea(LawyerPracticeAreas practice)
        {
            context.LawyerPracticeAreas.Add(practice);
            context.SaveChanges();
            return RedirectToAction("PracticeAreaList");
        }

        public ActionResult EditPracticeArea(int id)
        {
            var practice = context.LawyerPracticeAreas.SingleOrDefault(f => f.Id == id);
            return PartialView(practice);
        }
        [HttpPost]
        public ActionResult EditPracticeArea(LawyerPracticeAreas practice)
        {
            var faqsInDb = context.LawyerPracticeAreas.SingleOrDefault(f => f.Id == practice.Id);
            faqsInDb.Name = practice.Name;
            context.SaveChanges();
            return RedirectToAction("PracticeAreaList", "Admin");
        }

        [HttpPost]
        public ActionResult DeletePracticeArea(int id)
        {
            var faq = context.LawyerPracticeAreas.SingleOrDefault(f => f.Id == id);
            context.LawyerPracticeAreas.Remove(faq);
            context.SaveChanges();

            return RedirectToAction("PracticeAreaList", "Admin");
        }

        [HttpGet]
        public ActionResult LawyerLocationList()
        {
            var locations = context.LawyerLocations.ToList();
            return View(locations);
        }
        [HttpGet]
        public ActionResult CreateLawyerLocation()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateLawyerLocation(LawyerLocation location)
        {
            context.LawyerLocations.Add(location);
            context.SaveChanges();
            return RedirectToAction("LawyerLocationList");
        }

        public ActionResult EditLawyerLocation(int id)
        {
            var practice = context.LawyerLocations.SingleOrDefault(f => f.Id == id);
            return PartialView(practice);
        }
        [HttpPost]
        public ActionResult EditLawyerLocation(LawyerLocation practice)
        {
            var faqsInDb = context.LawyerLocations.SingleOrDefault(f => f.Id == practice.Id);
            faqsInDb.Name = practice.Name;
            context.SaveChanges();
            return RedirectToAction("LawyerLocationList", "Admin");
        }

        [HttpPost]
        public ActionResult DeleteLawyerLocation(int id)
        {
            var faq = context.LawyerLocations.SingleOrDefault(f => f.Id == id);
            context.LawyerLocations.Remove(faq);
            context.SaveChanges();

            return RedirectToAction("LawyerLocationList", "Admin");
        }
    }
}