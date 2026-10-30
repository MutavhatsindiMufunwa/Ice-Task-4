using StudentLibrary;

namespace StudentLibraryApi.Models
{
    public sealed record StudentResponse(string StudentNumber,string FullName,string Programme,int YearOfStudy,decimal AverageMark,bool IsActive)
    {
        public static StudentResponse FromStudent(Student student)
        {
            return new StudentResponse(student.StudentNumber, student.FullName, student.Programme, student.YearOfStudy, student.AverageMark, student.IsActive);
        }

    }
}
