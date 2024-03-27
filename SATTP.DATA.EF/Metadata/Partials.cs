//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATTP.DATA.EF.Models//.Metadata
{
    //internal class Partials
    //{
    //}

    [ModelMetadataType(typeof(CourseMetadata))]
    public partial class Course
    {

    }

    [ModelMetadataType(typeof(EnrollmentMetadata))]
    public partial class Enrollment
    {

    }

    [ModelMetadataType(typeof(ScheduledClassMetadata))]
    public partial class ScheduledClass
    {
        //For this to work, see the code in Enrollments controller.  We will have to include the courses when building the DDL.

        public string? ClassInfo
        {
            get
            {
                return Course == null ? null : $"{Course.CourseName} - {StartDate:d} - {Location}";
            }
        }
    }

    [ModelMetadataType(typeof(ScheduledClassStatusMetadata))]
    public partial class ScheduledClassStatus
    {

    }

    [ModelMetadataType(typeof(StudentMetadata))]
    public partial class Student
    {
        [NotMapped]
        public IFormFile? Image { get; set; }

        [Display(Name ="Student")]
        public string FullName { get { return FirstName + " " + LastName; } }
    }

    [ModelMetadataType(typeof(StudentStatusMetadata))]
    public partial class StudentStatus
    {

    }
}
