using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheVaqeel.Website.Models
{
    public class LawyerDetail
    {
        [Key]
        public string LawyerId { get; set; }
        [ForeignKey("LawyerId")]
        public ApplicationUser User { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string OfficeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int LawyerPracticeAreasId { get; set; }
        public LawyerPracticeAreas LawyerPracticeAreas { get; set; }
        public int LawyerLocationId { get; set; }
        public LawyerLocation LawyerLocation { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public byte IsValid { get; set; }
    }
}