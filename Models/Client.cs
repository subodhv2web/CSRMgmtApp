using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSRMGMT.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }  // Primary key for the corporate client

        [Required]
        [StringLength(250)]
        public string CompanyName { get; set; }  // Name of the company

        [Required]
        [StringLength(255)]
        public string Address { get; set; }  // Address of the company

        [StringLength(100)]
        public string City { get; set; }  // City where the company is located

        [StringLength(100)]
        public string State { get; set; }  // State or province where the company is located

        [StringLength(20)]
        public string PostalCode { get; set; }  // Postal or ZIP code

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }  // Phone number of the company

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }  // Email address of the company

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;  // Date when the record was created

        public DateTime? ModifiedDate { get; set; }  // Date when the record was last modified

        [StringLength(500)]
        public string? Notes { get; set; }  // Additional notes or comments

        [StringLength(100)]
        public string? Industry { get; set; }  // Industry in which the company operates

        [StringLength(250)]
        public string? Website { get; set; }  // Company website URL

        [Required]
        public string IsActive { get; set; }  // Status indicating if the client is active

        // SPOC Details
        [StringLength(100)]
        public string SPOCName { get; set; }  // Name of the Single Point of Contact (SPOC)

        [StringLength(50)]
        public string SPOCPhoneNumber { get; set; }  // Phone number of the SPOC

        [StringLength(255)]
        [EmailAddress]
        public string SPOCEmail { get; set; }  // Email address of the SPOC

        [StringLength(255)]
        public string SPOCPosition { get; set; }  // Position or job title of the SPOC

        public int ClientTypeId { get; set; }

        [NotMapped]
        public string? ClientType { get; set; }
    }
}
