using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SATTP.DATA.EF.Models
{
    public partial class ScheduledClass
    {
        public ScheduledClass()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Key]
        public int ScheduledClassID { get; set; }
        public int CourseId { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        [StringLength(40)]
        public string InstructorName { get; set; } = null!;
        [StringLength(20)]
        public string Location { get; set; } = null!;
        [Column("SCSID")]
        public int SCSID { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("ScheduledClasses")]
        public virtual Course? Course { get; set; } = null!;
        [ForeignKey("SCSID")]
        [InverseProperty("ScheduledClasses")]
        public virtual ScheduledClassStatus? ScheduledClassStatus { get; set; } = null!;
        [InverseProperty("ScheduledClass")]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
