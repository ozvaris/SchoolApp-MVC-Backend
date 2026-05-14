using Microsoft.AspNetCore.Mvc;
using SchoolApp_Backend.Dtos.CourseEnrollments;
using SchoolApp_Backend.Dtos.Students;
using SchoolApp_Backend.Services.Interfaces;

namespace SchoolApp_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseEnrollmentsController : ControllerBase
{
    private readonly ICourseEnrollmentService _courseEnrollmentService;

    public CourseEnrollmentsController(ICourseEnrollmentService courseEnrollmentService)
    {
        _courseEnrollmentService = courseEnrollmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseEnrollmentReadDto>>> GetAll([FromQuery] int? courseId)
    {
        var enrollments = courseId.HasValue
            ? await _courseEnrollmentService.GetByCourseIdAsync(courseId.Value)
            : await _courseEnrollmentService.GetAllAsync();

        return Ok(enrollments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseEnrollmentReadDto>> GetById(int id)
    {
        var enrollment = await _courseEnrollmentService.GetByIdAsync(id);
        return enrollment is null ? NotFound() : Ok(enrollment);
    }

    [HttpGet("available-students/{courseId:int}")]
    public async Task<ActionResult<IReadOnlyList<StudentReadDto>>> GetAvailableStudents(int courseId)
    {
        var students = await _courseEnrollmentService.GetAvailableStudentsByCourseIdAsync(courseId);
        return Ok(students);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseEnrollmentCreateDto dto)
    {
        var result = await _courseEnrollmentService.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CourseEnrollmentUpdateDto dto)
    {
        if (id != dto.CourseEnrollmentID)
        {
            return BadRequest(new { errorMessage = "Course enrollment id mismatch." });
        }

        var result = await _courseEnrollmentService.UpdateAsync(id, dto);
        if (!result.Success)
        {
            return result.ErrorMessage == "Course enrollment not found."
                ? NotFound()
                : BadRequest(new { errorMessage = result.ErrorMessage });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _courseEnrollmentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
