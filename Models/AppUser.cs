using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactPro.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be betwen {2} and {1} characters.", MinimumLength = 2)]
        public string? FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "The {0} must be betwen {2} and {1} characters.", MinimumLength = 2)]
        public string? LastName { get; set; }

        [NotMapped]
        [Display(Name = "Name")]
        public string? FullName { get { return $"{FirstName} {LastName}"; } }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    }
}
