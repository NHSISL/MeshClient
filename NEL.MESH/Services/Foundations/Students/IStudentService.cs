using System.Threading.Tasks;
using NEL.MESH.Models.Students;

namespace NEL.MESH.Services.Foundations.Students
{
    public interface IStudentService
    {
        ValueTask<Student> AddStudentAsync(Student student);
    }
}