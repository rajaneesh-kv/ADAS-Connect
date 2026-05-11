using System.ComponentModel.DataAnnotations;

namespace CvTracker.Web.Models
{
    public enum SchoolGrade
    {
        [Display(Name = "KG 1")]
        Kg1 = 1,

        [Display(Name = "KG 2")]
        Kg2 = 2,

        [Display(Name = "Grade 1")]
        Grade1 = 3,

        [Display(Name = "Grade 2")]
        Grade2 = 4,

        [Display(Name = "Grade 3")]
        Grade3 = 5,

        [Display(Name = "Grade 4")]
        Grade4 = 6,

        [Display(Name = "Grade 5")]
        Grade5 = 7,

        [Display(Name = "Grade 6")]
        Grade6 = 8,

        [Display(Name = "Grade 7")]
        Grade7 = 9,

        [Display(Name = "Grade 8")]
        Grade8 = 10,

        [Display(Name = "Grade 9")]
        Grade9 = 11,

        [Display(Name = "Grade 10")]
        Grade10 = 12,

        [Display(Name = "Grade 11")]
        Grade11 = 13,

        [Display(Name = "Grade 12")]
        Grade12 = 14
    }
}
