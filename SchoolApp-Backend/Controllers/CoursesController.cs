using Microsoft.AspNetCore.Mvc;
using SchoolApp_Backend.Dtos.Courses;
using SchoolApp_Backend.Services.Interfaces;

namespace SchoolApp_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseReadDto>>> GetAll()
    {
        var courses = await _courseService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseReadDto>> GetById(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        var result = await _courseService.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CourseUpdateDto dto)
    {
        if (id != dto.CourseID)
        {
            return BadRequest(new { errorMessage = "Course id mismatch." });
        }

        var result = await _courseService.UpdateAsync(id, dto);
        if (!result.Success)
        {
            return result.ErrorMessage == "Course not found."
                ? NotFound()
                : BadRequest(new { errorMessage = result.ErrorMessage });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _courseService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
