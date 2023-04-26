using ContactPro.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactPro.Models
{
    public class Contact
    {
        public int Id { get; set; }


        [Required]
        public string? AppUserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName { get { return $"{FirstName} {LastName}"; } }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string? Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }


        [Required]
        [Display(Name = "State")]
        public States States { get; set; }



        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }



        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Create Date")]
        public DateTime DateCreated { get; set; }


        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        // virtual
        public virtual AppUser? AppUser { get; set; }


        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    }
}
