using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SATTP.DATA.EF.Models
{
    public partial class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int ScheduledClassID { get; set; }
        [Column(TypeName = "date")]
        public DateTime EnrollmentDate { get; set; }

        [ForeignKey("ScheduledClassID")]
        [InverseProperty("Enrollments")]
        public virtual ScheduledClass? ScheduledClass { get; set; } = null!;
        [ForeignKey("StudentId")]
        [InverseProperty("Enrollments")]
        public virtual Student? Student { get; set; } = null!;
    }
}
