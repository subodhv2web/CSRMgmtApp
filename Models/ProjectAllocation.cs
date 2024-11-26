using System.ComponentModel.DataAnnotations;

namespace CSRMGMT.Models
{
    public class ProjectAllocation
    {
        [Key]
        public int Id { get; set; } 
        public int ProjectId { get; set; } // Foreign key to Project
        public CsrProject? Project { get; set; } // Navigation property for Project
        public int AgencyId { get; set; } // Foreign key to Agency
        public ProjectAgency? Agency { get; set; } // Navigation property for Agency
        public DateTime AllocationDate { get; set; } // Date of allocation (default to current date)
        public string? Remarks { get; set; } // Optional remarks for the allocation
    }
}
