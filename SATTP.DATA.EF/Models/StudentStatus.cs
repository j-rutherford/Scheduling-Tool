using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SATTP.DATA.EF.Models
{
    public partial class StudentStatus
    {
        public StudentStatus()
        {
            Students = new HashSet<Student>();
        }

        [Key]
        [Column("SSID")]
        public int SSID { get; set; }
        [Column("SSName")]
        [StringLength(30)]
        public string SSName { get; set; } = null!;
        [Column("SSDescription")]
        [StringLength(250)]
        public string? SSDescription { get; set; }

        [InverseProperty("StudentStatus")]
        public virtual ICollection<Student> Students { get; set; }
    }
}
