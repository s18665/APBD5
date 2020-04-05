using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentServiceDb _service;
        
        public EnrollmentsController(IStudentServiceDb service)
        {
            _service = service;
        }
        
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var response = _service.EnrollStudent(request);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
    
        [HttpPost("promote")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            var response = _service.PromoteStudents(request);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound();
        }
    }
}