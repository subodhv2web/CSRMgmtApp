using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSRMGMT.Models
{
    public class LinkTypes
    {
        [Key]
        public int LinkTypeId { get; set; }
        [StringLength(250)]
        public string LinkTypeName { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
