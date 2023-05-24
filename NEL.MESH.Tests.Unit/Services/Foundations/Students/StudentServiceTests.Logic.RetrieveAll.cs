using System.Linq;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Students;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public void ShouldReturnStudents()
        {
            // given
            IQueryable<Student> randomStudents = CreateRandomStudents();
            IQueryable<Student> storageStudents = randomStudents;
            IQueryable<Student> expectedStudents = storageStudents;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudents())
                    .Returns(storageStudents);

            // when
            IQueryable<Student> actualStudents =
                this.studentService.RetrieveAllStudents();

            // then
            actualStudents.Should().BeEquivalentTo(expectedStudents);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudents(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}