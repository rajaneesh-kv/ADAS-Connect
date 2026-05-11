using System;
using System.ComponentModel.DataAnnotations;

namespace CvTracker.Web.Models
{
    public class SchoolRegistration
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string ChildName { get; set; }

        [Required]
        public SchoolGrade Grade { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(260)]
        public string PhotoPath { get; set; }

        [Required]
        [MaxLength(120)]
        public string FatherName { get; set; }

        [Required]
        [MaxLength(120)]
        public string FatherProfession { get; set; }

        [Required]
        [MaxLength(30)]
        public string FatherMobile { get; set; }

        [Required]
        [MaxLength(150)]
        public string FatherEmail { get; set; }

        [MaxLength(30)]
        public string FatherLandline { get; set; }

        [Required]
        [MaxLength(120)]
        public string MotherName { get; set; }

        [Required]
        [MaxLength(120)]
        public string MotherProfession { get; set; }

        [Required]
        [MaxLength(30)]
        public string MotherMobile { get; set; }

        [Required]
        [MaxLength(150)]
        public string MotherEmail { get; set; }

        [MaxLength(30)]
        public string MotherLandline { get; set; }

        [Required]
        [MaxLength(200)]
        public string AreaOfLiving { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
