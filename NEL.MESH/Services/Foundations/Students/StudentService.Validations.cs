using NEL.MESH.Models.Students;
using NEL.MESH.Models.Students.Exceptions;

namespace NEL.MESH.Services.Foundations.Students
{
    public partial class StudentService
    {
        private void ValidateStudentOnAdd(Student student)
        {
            ValidateStudentIsNotNull(student);
        }

        private static void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }
    }
}