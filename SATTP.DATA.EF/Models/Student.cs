using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SATTP.DATA.EF.Models
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Key]
        public int StudentId { get; set; }
        [StringLength(20)]
        public string FirstName { get; set; } = null!;
        [StringLength(20)]
        public string LastName { get; set; } = null!;
        [StringLength(15)]
        public string? Major { get; set; }
        [StringLength(50)]
        public string? Address { get; set; }
        [StringLength(25)]
        public string? City { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string? State { get; set; }
        [StringLength(10)]
        public string? ZipCode { get; set; }
        [StringLength(13)]
        public string? Phone { get; set; }
        [StringLength(60)]
        public string Email { get; set; } = null!;
        [StringLength(100)]
        public string? PhotoUrl { get; set; }
        [Column("SSID")]
        public int SSID { get; set; }

        [ForeignKey("SSID")]
        [InverseProperty("Students")]
        public virtual StudentStatus? StudentStatus { get; set; } = null!;
        [InverseProperty("Student")]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
