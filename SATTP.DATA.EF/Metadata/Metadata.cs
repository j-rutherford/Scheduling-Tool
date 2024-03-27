using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SATTP.DATA.EF.Models//.Metadata
{
    //internal class Metadata
    //{
    //}
    public class CourseMetadata // make it public
    {
        [Display(Name = "Course")]
        public string CourseName { get; set; } = null!;

        [Display(Name = "Description")]
        [UIHint("MultilineText")]
        public string CourseDescription { get; set; } = null!;

        [Display(Name = "Credit Hours")]
        [Range(0, 15, ErrorMessage = "* Must be greater than 0 and less than 15")] 
        public byte CreditHours { get; set; }

        [Display(Name = "Curriculum")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Curriculum { get; set; }

        [DisplayFormat(NullDisplayText = "No Additional Info")]
        [UIHint("MultilineText")]
        public string? Notes { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        //public virtual ICollection<ScheduledClass> ScheduledClasses { get; set; }
    }

    public class EnrollmentMetadata
    {
        //public int EnrollmentId { get; set; }
        //public int StudentId { get; set; }
        //public int ScheduledClassID { get; set; }

        [Display(Name = "Date Enrolled")]
        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.DateTime, ErrorMessage = "* Must be in correct date format (mm/dd/yyyy)")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public DateTime EnrollmentDate { get; set; }
    }


    public class ScheduledClassMetadata
    {
        //public int ScheduledClassID { get; set; }
        //public int CourseId { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }

        [Display(Name = "Instructor")]
        public string InstructorName { get; set; }
    }


    public class ScheduledClassStatusMetadata
    {

        [Display(Name = "Schedule Class Status")]
        public string SCSName { get; set; } = null!;
    }


    public class StudentMetadata
    {
        //public int StudentId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Major { get; set; }

        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Address { get; set; }
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? City { get; set; }
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? State { get; set; }
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? ZipCode { get; set; }
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Display(Name = "Photo")]
        public string? PhotoUrl { get; set; }

        //public int SSID { get; set; }
    }


    public class StudentStatusMetadata
    {
        //public int SSID { get; set; }

        [Display(Name = "Student Status")]
        public string SSName { get; set; } = null!;

        [Display(Name = "Status Description")]
        public string? SSDescription { get; set; }
    }
}
