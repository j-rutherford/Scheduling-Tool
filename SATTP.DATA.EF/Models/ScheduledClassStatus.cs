using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SATTP.DATA.EF.Models
{
    public partial class ScheduledClassStatus
    {
        public ScheduledClassStatus()
        {
            ScheduledClasses = new HashSet<ScheduledClass>();
        }

        [Key]
        [Column("SCSID")]
        public int SCSID { get; set; }
        [Column("SCSName")]
        [StringLength(50)]
        [Unicode(false)]
        public string SCSName { get; set; } = null!;

        [InverseProperty("ScheduledClassStatus")]
        public virtual ICollection<ScheduledClass> ScheduledClasses { get; set; }
    }
}
