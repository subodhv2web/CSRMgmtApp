using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSRMGMT.Areas.Admin;

namespace CSRMGMT.Models
{
    public class LinksInfo
    {
        [Key]
        public int sLinkID { get; set; }

        [Required]
        [StringLength(500)]
        public string sLinkName { get; set; }

        public int? ParentLinkID { get; set; }
        public bool UseURL { get; set; }

        [StringLength(250)]
        [RequiredIf("UseURL", ErrorMessage = "Ex Url is required.")]
        public string? ExURL { get; set; }

        [StringLength(250)]
        public string? ImagePath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LinkContents { get; set; }

        public bool IsLinkActive { get; set; }

        public bool InNewWindow { get; set; }

        public DateTime LinkCreatedOn { get; set; }

        public DateTime LinkLastUpdate { get; set; }

        [StringLength(250)]
        public string? WindowTitle { get; set; }

        [StringLength(250)]
        public string? MetaKeywords { get; set; }

        [StringLength(250)]
        public string? MetaDescription { get; set; }

        public int? sLinkTypeID { get; set; }

        [ForeignKey("sLinkTypeID")]
        public LinkTypes? LinkTypes { get; set; } = null;
    }
}
