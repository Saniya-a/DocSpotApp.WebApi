using DocSpotApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocSpotApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public decimal? Fees { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public int? HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
