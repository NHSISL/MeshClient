using System;
using Xeptions;

namespace NEL.MESH.Models.Students.Exceptions
{
    public class StudentServiceException : Xeption
    {
        public StudentServiceException(Exception innerException)
            : base(message: "Student service error occurred, contact support.", innerException)
        { }
    }
}