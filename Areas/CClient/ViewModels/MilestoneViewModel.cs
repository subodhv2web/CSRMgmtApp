using CSRMGMT.Models;

namespace CSRMGMT.Areas.CClient.ViewModels
{
    public class MilestoneViewModel
    {
        public Milestone Milestone { get; set; }  // For creating or editing a single milestone
        public List<Milestone>? MilestoneList { get; set; }  // List of all milestones for display
    }

}
