using System.Threading.Tasks;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Loggings;
using NEL.MESH.Brokers.Storages;
using NEL.MESH.Models.Students;

namespace NEL.MESH.Services.Foundations.Students
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Student> AddStudentAsync(Student student) =>
            await this.storageBroker.InsertStudentAsync(student);
    }
}