using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheVaqeel.Website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public byte Status { get; set; }
        public byte IsValid { get; set; }   
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("TheVaqeelConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new TheVaqeelDbInitializer());    
        }
        public DbSet<HomeNavbar> HomeNavbars { get; set; }
        public DbSet<HomeSlider> HomeSliders { get; set; }
        public DbSet<Faqs> Faqs{ get; set; }
        public DbSet<LawyerServiceContent> LawyerServiceContents{ get; set; }
        public DbSet<LawyerLocation> LawyerLocations { get; set; }
        public DbSet<LawyerPracticeAreas> LawyerPracticeAreas { get; set; }
        public DbSet<LawyerDetail> LawyerDetails { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}