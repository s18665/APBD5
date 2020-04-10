using System;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IStudentServiceDb
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request);
        bool CheckIndex(String index);
    }
}