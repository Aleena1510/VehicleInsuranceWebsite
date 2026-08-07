using System;
using System.ComponentModel.DataAnnotations;

namespace FormConfirmationReport.Models
{
    /// <summary>
    /// Model representing all data filled by the user in the application form.
    /// Adjust fields to match your actual form requirements.
    /// </summary>
    public class ApplicationFormModel
    {
        // ── Personal Information ────────────────────────────────────────────
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "CNIC / ID Number")]
        public string CNIC { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        // ── Contact Details ─────────────────────────────────────────────────
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Residential Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Province / State")]
        public string Province { get; set; }

        // ── Application Details ─────────────────────────────────────────────
        [Display(Name = "Application Type")]
        public string ApplicationType { get; set; }

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; } = "REF-" + DateTime.Now.ToString("yyyyMMddHHmmss");

        [Display(Name = "Additional Notes")]
        [DataType(DataType.MultilineText)]
        public string AdditionalNotes { get; set; }
    }
}
