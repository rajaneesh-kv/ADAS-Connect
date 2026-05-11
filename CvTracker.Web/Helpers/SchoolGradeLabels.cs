using CvTracker.Web.Models;

namespace CvTracker.Web.Helpers
{
    public static class SchoolGradeLabels
    {
        public static string For(SchoolGrade grade)
        {
            return grade switch
            {
                SchoolGrade.Kg1 => "KG 1",
                SchoolGrade.Kg2 => "KG 2",
                SchoolGrade.Grade1 => "Grade 1",
                SchoolGrade.Grade2 => "Grade 2",
                SchoolGrade.Grade3 => "Grade 3",
                SchoolGrade.Grade4 => "Grade 4",
                SchoolGrade.Grade5 => "Grade 5",
                SchoolGrade.Grade6 => "Grade 6",
                SchoolGrade.Grade7 => "Grade 7",
                SchoolGrade.Grade8 => "Grade 8",
                SchoolGrade.Grade9 => "Grade 9",
                SchoolGrade.Grade10 => "Grade 10",
                SchoolGrade.Grade11 => "Grade 11",
                SchoolGrade.Grade12 => "Grade 12",
                _ => grade.ToString()
            };
        }
    }
}
