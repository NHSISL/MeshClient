using System;
using Xeptions;

namespace NEL.MESH.Models.Students.Exceptions
{
    public class FailedStudentServiceException : Xeption
    {
        public FailedStudentServiceException(Exception innerException)
            : base(message: "Failed student service occurred, please contact support", innerException)
        { }
    }
}