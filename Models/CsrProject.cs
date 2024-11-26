using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSRMGMT.Models
{
    public class CsrProject
    {
        public int Id { get; set; }  // Unique identifier for the CSR project

        [StringLength(500)]
        public string ProjectName { get; set; }  // Name of the CSR project

        public string Description { get; set; }  // Detailed description of the project
        public DateTime StartDate { get; set; }  // Date when the project started
        public DateTime? EndDate { get; set; }  // Date when the project ended (nullable)

        [StringLength(50)]
        public string Status { get; set; }  // Current status of the project (e.g., Ongoing, Completed, Planned)
        public int? ProjectCategoryId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number.")]
        public decimal Budget { get; set; }  // Budget allocated for the project
        [StringLength(255)]
        public string ContactPerson { get; set; }  // Person responsible for the project

        [StringLength(250)]
        [EmailAddress]
        public string ContactEmail { get; set; }  // Email address of the contact person

        [StringLength(20)]
        public string ContactPhone { get; set; }  // Phone number of the contact person

        [StringLength(500)]
        public string Location { get; set; }  // Location where the project is implemented

        // File upload
        [NotMapped]
        public IFormFile? FileUpload { get; set; }  // The file being uploaded

        // File Path
        [StringLength(500)]
        public string? FilePath { get; set; }  // Path to the uploaded file

        // Milestones
        public List<Milestone>? Milestone { get; set; }  // List of milestones associated with the project
    }
    public class Milestone
    {
        [Key]
        public int Id { get; set; }  // Unique identifier for the milestone
        public int CsrProjectId { get; set; }  // Foreign key to the CSR project

        public string Description { get; set; }  // Description of the milestone
        public DateTime TargetDate { get; set; } = DateTime.Now.Date; // Target date for the milestone

        [StringLength(50)]
        public string Status { get; set; }  // Status of the milestone (e.g., Pending, Completed)
        
        [StringLength(500)]
        public string StatusDescription { get; set; }

        [NotMapped]
        public IFormFile? StatusFileUpload { get; set; }  // The file being uploaded

        // File Path
        [StringLength(500)]
        public string? StatusFilePath { get; set; }  // Path to the uploaded file
        public CsrProject? CsrProject { get; set; }  // Navigation property
    }
}
