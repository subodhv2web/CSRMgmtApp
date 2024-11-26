using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSRMGMT.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(150)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? UserRole { get; set; }
       
        [StringLength(20)]
        public string? MobileNo { get; set; }

        [StringLength(50)]
        public string? Password { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedOn { get; set; }
       
    }
}
