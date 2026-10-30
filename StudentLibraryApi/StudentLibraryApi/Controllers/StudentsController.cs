using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using StudentLibrary;
using StudentLibraryApi.Models;

namespace StudentLibraryApi.Controllers
{
    [ApiController]
    [Route("api/students")]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDataService _studentService;

        public StudentsController(StudentDataService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentService.GetAllStudents();

            var results = students
                .Select(StudentResponse.FromStudent)
                .ToList();

            return Ok(results);
        }

        [HttpGet("number/{studentNumber}")]
        public IActionResult GetByStudentNumber(string studentNumber)
        {
            if (string.IsNullOrWhiteSpace(studentNumber))
            {
                return BadRequest(
                    new ApiError("Student number cannot be blank."));
            }

            try
            {
                var student = _studentService
                    .GetByStudentNumber(studentNumber.Trim());

                if (student == null)
                {
                    return NotFound(
                        new ApiError("No student was found with that student number."));
                }

                return Ok(StudentResponse.FromStudent(student));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(ex.Message));
            }
        }

        [HttpGet("programme/{programme}")]
        public IActionResult GetByProgramme(string programme)
        {
            if (string.IsNullOrWhiteSpace(programme))
            {
                return BadRequest(
                    new ApiError("Programme cannot be blank."));
            }

            try
            {
                var students = _studentService
                    .GetByProgramme(programme.Trim());

                if (students.Count == 0)
                {
                    return NotFound(
                        new ApiError("No students were found for that programme."));
                }

                var results = students
                    .Select(StudentResponse.FromStudent)
                    .ToList();

                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(ex.Message));
            }
        }

        [HttpGet("search")]
        public IActionResult SearchByName([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(
                    new ApiError("A non-blank name search term is required."));
            }

            try
            {
                var students = _studentService
                    .SearchByName(name.Trim());

                var results = students
                    .Select(StudentResponse.FromStudent)
                    .ToList();

                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(ex.Message));
            }
        }

        [HttpGet("top")]
        public IActionResult GetTopStudents([FromQuery] int count = 5)
        {
            if (count <= 0)
            {
                return BadRequest(
                    new ApiError("Count must be greater than zero."));
            }

            try
            {
                var students = _studentService
                    .GetTopStudents(count);

                var results = students
                    .OrderByDescending(student => student.AverageMark)
                    .Select(StudentResponse.FromStudent)
                    .ToList();

                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(ex.Message));
            }
        }

        [HttpGet("programme/{programme}/average")]
        public IActionResult GetProgrammeAverage(string programme)
        {
            if (string.IsNullOrWhiteSpace(programme))
            {
                return BadRequest(
                    new ApiError("Programme cannot be blank."));
            }

            try
            {
                string cleanedProgramme = programme.Trim();

                decimal average = _studentService
                    .GetProgrammeAverage(cleanedProgramme);

                var result = new ProgrammeAverageResponse(
                    cleanedProgramme.ToUpperInvariant(),
                    Math.Round(average, 2),
                    average.ToString("F2", CultureInfo.InvariantCulture)
                );

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError(ex.Message));
            }

        }
    }
}
