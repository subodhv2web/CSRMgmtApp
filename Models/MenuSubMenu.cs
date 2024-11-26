using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSRMGMT.Models
{
    public class MenuSubMenu
    {
        [Key]
        public int sLinkID { get; set; }
        [Required]
        
        [StringLength(250)]
        public string sLinkName { get; set; }

        [StringLength(250)]
        public string sClass { get; set; }

        [StringLength(250)]
        public string slug { get; set; }

        [StringLength(500)]
        public string ExURL { get; set; }
        public int hasSubmenu { get; set; }

        public bool UseURL { get; set; } = false;

        public List<LinksInfo>Submenu { get; set; }
}
}
