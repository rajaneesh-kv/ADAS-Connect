using System;
using System.ComponentModel.DataAnnotations;
using CvTracker.Web.Models;
using Microsoft.AspNetCore.Http;

namespace CvTracker.Web.ViewModels
{
    public class SchoolRegistrationCreateViewModel
    {
        [Required(ErrorMessage = "Child name is required.")]
        [Display(Name = "Child's full name")]
        [MaxLength(120)]
        public string ChildName { get; set; }

        [Required(ErrorMessage = "Please choose a grade.")]
        [Display(Name = "Grade / class")]
        public SchoolGrade? Grade { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of birth (DD/MM/YYYY)")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Child's photo (optional)")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Father's name is required.")]
        [Display(Name = "Father's full name")]
        [MaxLength(120)]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Father's profession is required.")]
        [Display(Name = "Father's profession")]
        [MaxLength(120)]
        public string FatherProfession { get; set; }

        [Required(ErrorMessage = "Father's mobile is required.")]
        [Display(Name = "Father's mobile / WhatsApp")]
        [MaxLength(30)]
        public string FatherMobile { get; set; }

        [Required(ErrorMessage = "Father's email is required.")]
        [Display(Name = "Father's email")]
        [EmailAddress]
        [MaxLength(150)]
        public string FatherEmail { get; set; }

        [Display(Name = "Father's landline (optional)")]
        [MaxLength(30)]
        public string FatherLandline { get; set; }

        [Required(ErrorMessage = "Mother's name is required.")]
        [Display(Name = "Mother's full name")]
        [MaxLength(120)]
        public string MotherName { get; set; }

        [Required(ErrorMessage = "Mother's profession is required.")]
        [Display(Name = "Mother's profession")]
        [MaxLength(120)]
        public string MotherProfession { get; set; }

        [Required(ErrorMessage = "Mother's mobile is required.")]
        [Display(Name = "Mother's mobile / WhatsApp")]
        [MaxLength(30)]
        public string MotherMobile { get; set; }

        [Required(ErrorMessage = "Mother's email is required.")]
        [Display(Name = "Mother's email")]
        [EmailAddress]
        [MaxLength(150)]
        public string MotherEmail { get; set; }

        [Display(Name = "Mother's landline (optional)")]
        [MaxLength(30)]
        public string MotherLandline { get; set; }

        [Required(ErrorMessage = "Area of living is required.")]
        [Display(Name = "Area of living")]
        [MaxLength(200)]
        public string AreaOfLiving { get; set; }
    }
}
