using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSRMGMT.Areas.Admin.ViewModels
{
    public class LinksInfoViewModel
    {
        public int sLinkID { get; set; }
        public string sLinkName { get; set; }
        public bool UseURL { get; set; }
        public string? ExURL { get; set; }

        public string? ImagePath { get; set; }
        public string LinkContents { get; set; }
        public string sLinkStatus { get; set; }

        public bool InNewWindow { get; set; }

        public DateTime LinkLastUpdate { get; set; }

        public string? WindowTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public string sLinkStatusClass { get; set; }
        public string? linkType { get; set; }
    }
}
