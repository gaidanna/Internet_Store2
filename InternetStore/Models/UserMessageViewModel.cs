using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace InternetStore.Models
{
    public class UserMessageViewModel
    {
        [Required(ErrorMessage="Please enter Your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter Your Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage="Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Your phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter Your question or proposition")]
        public string Comments { get; set; }

        public string Complete()
        {
            return string.Format("{0} {1} \n {2} \n {3} \n {4} \n {5}", FirstName, LastName, Email, Phone, Comments, DateTime.Now);
        }
    }
}