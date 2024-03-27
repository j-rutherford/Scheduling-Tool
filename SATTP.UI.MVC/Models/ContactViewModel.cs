using System.ComponentModel.DataAnnotations; //added

namespace SATTP.UI.MVC.Models
{
    //benefits of using a ViewModel
    //- collect and/or display info on a view ("complex" - multiple pieces of info)
    //- can use EF to create the view (scaffolding)
    //- server-side validation
    public class ContactViewModel //view model to collect and process contact info
    {
        //name - who are you?
        //email - how can we contact you?
        //message - reason for contact
        [Required(ErrorMessage = "* Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        //validate email via RegEx or [DataType]
        public string Email { get; set; }

        [UIHint("MultilineText")] //textarea instead of textbox (MVC controls)
        [Required(ErrorMessage = "* Required")]
        public string Message { get; set; }
    }

    //! scaffold view - Contact.cshtml
}