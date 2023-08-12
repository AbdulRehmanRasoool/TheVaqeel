using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TheVaqeel.Website.Models
{
    public class TheVaqeelDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            SeedFaqs(context);
            SeedNavbar(context);
            SeedSlider(context);
            SeedLawyerService(context);
            SeedLawyerLocation(context);
            SeedLawyerPracticeAreas(context);
            SeedTimeSlot(context);
        }
        public void SeedRoles(ApplicationDbContext context)
        {
            List<IdentityRole> rolesInTheVaqeel = new List<IdentityRole>();

            rolesInTheVaqeel.Add(new IdentityRole() { Name = "Administrator" });
            rolesInTheVaqeel.Add(new IdentityRole() { Name = "Lawyer" });
            rolesInTheVaqeel.Add(new IdentityRole() { Name = "User" });

            var rolesStore = new RoleStore<IdentityRole>(context);
            var rolesManager = new RoleManager<IdentityRole>(rolesStore);

            foreach (IdentityRole role in rolesInTheVaqeel)
            {
                if (!rolesManager.RoleExists(role.Name))
                {
                    var result = rolesManager.Create(role);
                    if (result.Succeeded) continue;
                }
            }
        }
        public void SeedUsers(ApplicationDbContext context)
        {
            var usersStore = new UserStore<ApplicationUser>(context);
            var usersManager = new UserManager<ApplicationUser>(usersStore);

            var admin = new ApplicationUser();
            admin.Email = "admin@email.com";
            admin.UserName = "admin@email.com";
            var password = "admin123";

            if(usersManager.FindByEmail(admin.Email) == null)
            {
                var result = usersManager.Create(admin,password);
                if (result.Succeeded)
                {
                    usersManager.AddToRole(admin.Id, "Administrator");
                }
            }
        }
        public void SeedFaqs(ApplicationDbContext context)
        {
            context.Faqs.Add(new Faqs()
            {
                Question = "Demo Question",
                Answer = "Demo Answer"
            });
        }
        public void SeedNavbar(ApplicationDbContext context)
        {
            context.HomeNavbars.Add(new HomeNavbar()
            {
                PhoneNumber = "0000111122",
                Email = "dummy@email.com",
                Address = "dummy",
                Logo = "dummyLogo",
                Location = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d115771.59367132922!2d67.010058206468!3d24.936760382404973!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3eb36a97e7a82339%3A0xdc35b559a186df79!2sIqbal%20Market%20(Pakistan%20Bazar)!5e0!3m2!1sen!2s!4v1690712275030!5m2!1sen!2s\" width=\"600\" height=\"450\" style=\"border:0;\" allowfullscreen=\"\" loading=\"lazy\" referrerpolicy=\"no-referrer-when-downgrade"
            });
        }
        public void SeedSlider(ApplicationDbContext context)
        {
            context.HomeSliders.Add(new HomeSlider()
            {
                Image = "Demo",
                Title = "Demo",
                Description = "Demo"
            });
        }
        public void SeedLawyerService(ApplicationDbContext context)
        {
            context.LawyerServiceContents.Add(new LawyerServiceContent()
            {
                Image = "demo",
                Title = "demo",
                Description = "demo"
            });
        }
        public void SeedLawyerLocation(ApplicationDbContext context)
        {
            context.LawyerLocations.Add(new LawyerLocation()
            {
                Name = "High Court"
            });
        }
        public void SeedLawyerPracticeAreas(ApplicationDbContext context)
        {
            context.LawyerPracticeAreas.Add(new LawyerPracticeAreas()
            {
                Name = "Real State Lawyer"
            });
        }
        public void SeedTimeSlot(ApplicationDbContext context)
        {
            context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "9:00AM",
                EndTime = "10:00AM"
            });
            context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "10:00AM",
                EndTime = "11:00AM"
            });
            context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "11:00AM",
                EndTime = "12:00PM"
            });
            context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "01:00PM",
                EndTime = "02:00PM"
            });
            context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "02:00PM",
                EndTime = "03:00PM"
            });
            context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "03:00PM",
                EndTime = "04:00PM"
            }); context.TimeSlots.Add(new TimeSlot()
            {
                StartTime = "04:00PM",
                EndTime = "05:00PM"
            });
            context.SaveChanges();
        }
    }
}