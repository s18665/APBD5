using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IStudentServiceDb
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request);
    }
}