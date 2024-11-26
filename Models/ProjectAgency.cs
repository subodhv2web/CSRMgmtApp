using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSRMGMT.Models
{
    public class ProjectAgency
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(250)]
        public string Name { get; set; } // Name of the Agency

        [StringLength(500)]
        public string Address { get; set; } // Physical address

        [StringLength(100)]
        public string City { get; set; }
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public State? State { get; set; }

        [StringLength(15)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        public string ContactNumber { get; set; } // Main contact number

        [EmailAddress]
        public string Email { get; set; } // Contact email address

        public DateTime? EstablishedDate { get; set; } // Optional: When the agency was established

        [StringLength(500)]
        public string Description { get; set; } // Brief description or overview of the agency

        public bool IsActive { get; set; } = true; // To indicate if the agency is active

        // SPOC Details
        [StringLength(100)]
        public string SpocName { get; set; } // SPOC's name

        [StringLength(100)]
        public string SpocPosition { get; set; } // SPOC's position/title
        
        public string SpocContactNumber { get; set; } // SPOC's contact number

        [EmailAddress]
        public string SpocEmail { get; set; } // SPOC's email address
    }
}
