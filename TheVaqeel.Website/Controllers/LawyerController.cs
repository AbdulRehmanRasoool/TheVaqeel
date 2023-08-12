using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using TheVaqeel.Website.Models;
using TheVaqeel.Website.ViewModel;

namespace TheVaqeel.Website.Controllers
{
    public class LawyerController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            var lawyerId = User.Identity.GetUserId();
            var lawyer = context.LawyerDetails.SingleOrDefault(l => l.LawyerId == lawyerId);
            homeViewModel.LawyerDetail = lawyer;
            return View(homeViewModel);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var lawyerDetail = new LawyerDetail();
            var homeViewModel = new HomeViewModel();
            var navbarElements = context.HomeNavbars.SingleOrDefault(n => n.Id == 1);
            var lawyerLocations = context.LawyerLocations.ToList();
            var lawyerPracticeAreas = context.LawyerPracticeAreas.ToList();
            homeViewModel.HomeNavbar = navbarElements;
            homeViewModel.LawyerDetail = lawyerDetail;
            homeViewModel.LawyerLocation = lawyerLocations;
            homeViewModel.LawyerPracticeAreas = lawyerPracticeAreas;
            return View(homeViewModel);
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,LawyerDetail lawyerDetail)
        {
            var lawyerId = User.Identity.GetUserId();
            var fileName = Path.GetFileName(file.FileName);
            var _fileName = Guid.NewGuid() + fileName;
            var path = Path.Combine(Server.MapPath("~/Content/img/team"), _fileName);
            lawyerDetail.Image = _fileName;
            lawyerDetail.IsValid = 1;
            lawyerDetail.LawyerId = lawyerId;
            context.LawyerDetails.Add(lawyerDetail);
            if (context.SaveChanges() > 0)
            {
                file.SaveAs(path);
            }

            return RedirectToAction("Index","Lawyer");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var homeViewModel = new HomeViewModel();
            var lawyer = context.LawyerDetails.Include(l => l.LawyerPracticeAreas).Include(l => l.LawyerLocation).SingleOrDefault(l => l.LawyerId == id);
            Session["imgPath"] = lawyer.Image;
            homeViewModel.LawyerDetail = lawyer;
            var lawyerLocations = context.LawyerLocations.ToList();
            var lawyerPracticeAreas = context.LawyerPracticeAreas.ToList();
            homeViewModel.LawyerLocation = lawyerLocations;
            homeViewModel.LawyerPracticeAreas = lawyerPracticeAreas;
            return View(homeViewModel);
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, HomeViewModel homeSlider)
        {
            var sliderInDb = context.LawyerDetails.SingleOrDefault(s => s.LawyerId == homeSlider.LawyerDetail.LawyerId);
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var _fileName = Guid.NewGuid() + fileName;
                var extension = Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img/team/"), _fileName);

                homeSlider.LawyerDetail.Image = _fileName;

                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    if (file.ContentLength <= 1000000)
                    {
                        sliderInDb.FullName = homeSlider.LawyerDetail.FullName;
                        sliderInDb.State = homeSlider.LawyerDetail.State;
                        sliderInDb.MobileNumber = homeSlider.LawyerDetail.MobileNumber;
                        sliderInDb.City = homeSlider.LawyerDetail.City;
                        sliderInDb.Description = homeSlider.LawyerDetail.Description;
                        sliderInDb.LawyerLocationId = homeSlider.LawyerDetail.LawyerLocationId;
                        sliderInDb.LawyerPracticeAreasId = homeSlider.LawyerDetail.LawyerPracticeAreasId;
                        sliderInDb.OfficeAddress = homeSlider.LawyerDetail.OfficeAddress;
                        sliderInDb.Image = homeSlider.LawyerDetail.Image;
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
                sliderInDb.Image = Session["imgPath"].ToString();
                sliderInDb.FullName = homeSlider.LawyerDetail.FullName;
                sliderInDb.State = homeSlider.LawyerDetail.State;
                sliderInDb.MobileNumber = homeSlider.LawyerDetail.MobileNumber;
                sliderInDb.City = homeSlider.LawyerDetail.City;
                sliderInDb.Description = homeSlider.LawyerDetail.Description;
                sliderInDb.LawyerLocationId = homeSlider.LawyerDetail.LawyerLocationId;
                sliderInDb.LawyerPracticeAreasId = homeSlider.LawyerDetail.LawyerPracticeAreasId;
                sliderInDb.OfficeAddress = homeSlider.LawyerDetail.OfficeAddress;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult AppointmentList()
        {
            var lawyerId = User.Identity.GetUserId();

            var homeViewModel = new HomeViewModel();
            var lawyer = context.LawyerDetails.SingleOrDefault(l => l.LawyerId == lawyerId);
            homeViewModel.LawyerDetail = lawyer;
            var appointment = context.Appointments.Where(l => l.LawyerDetailId == lawyerId).Include(l => l.TimeSlot).ToList();
            homeViewModel.Appointments = appointment;
            if (Request.IsAjaxRequest())
            {
                return PartialView(homeViewModel);
            }
            return View(homeViewModel);
        }

        [HttpGet]
        public ActionResult ApproveAppointment(int id)
        {
            var appointment = context.Appointments.Include(l => l.LawyerDetail).Include(a => a.TimeSlot).SingleOrDefault(a => a.Id == id);
            if (appointment.Status == 1)
            {
                appointment.Status = 2;
                appointment.TimeSlot.IsAvailaible = false;

                string smtpServer = "smtp.gmail.com";
                string senderEmail = "thelawrexcompany@gmail.com";
                string senderPassword = "gywbbimaqvnxkggo";
                string recipientEmail = appointment.Email;
                string subject = "Appointment Completed Email";
                string body = "Thank You for your appointment. "; 

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
            else
            {
                appointment.Status = 1;
                appointment.TimeSlot.IsAvailaible = true;

                string smtpServer = "smtp.gmail.com";
                string senderEmail = "thelawrexcompany@gmail.com";
                string senderPassword = "gywbbimaqvnxkggo";
                string recipientEmail = appointment.Email;
                string subject = "Appointment Confirmation Email";
                string body = "Your appointment with lawyer  \n" + "Date : " + appointment.Date.ToShortDateString() + "\n" + "Start Time: " + appointment.TimeSlot.StartTime + "\n" + "End Time: " + appointment.TimeSlot.EndTime + "\n" + "Please be on time to avoid Cancellation"; 

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
            context.SaveChanges();
            return RedirectToAction("AppointmentList", "Lawyer");
        }
        [HttpGet]
        public ActionResult RejectAppointment(int id)
        {
            var appointment = context.Appointments.Include(l => l.LawyerDetail).Include(a => a.TimeSlot).SingleOrDefault(a => a.Id == id);
            appointment.Status = 3;
            appointment.TimeSlot.IsAvailaible = false;

            string smtpServer = "smtp.gmail.com";
            string senderEmail = "thelawrexcompany@gmail.com";
            string senderPassword = "gywbbimaqvnxkggo";
            string recipientEmail = appointment.Email;
            string subject = "Appointment Rejection Email";
            string body = "Your appointment has been cancelled due to some reasons.Please try again later";

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
            context.SaveChanges();
            return RedirectToAction("AppointmentList", "Lawyer");
        }
    }
}