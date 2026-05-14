using Microsoft.AspNetCore.Mvc;
using SchoolApp_Backend.Dtos.Students;
using SchoolApp_Backend.Services.Interfaces;

namespace SchoolApp_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StudentReadDto>>> GetAll()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentReadDto>> GetById(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        var result = await _studentService.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StudentUpdateDto dto)
    {
        if (id != dto.StudentID)
        {
            return BadRequest(new { errorMessage = "Student id mismatch." });
        }

        var result = await _studentService.UpdateAsync(id, dto);
        if (!result.Success)
        {
            return result.ErrorMessage == "Student not found."
                ? NotFound()
                : BadRequest(new { errorMessage = result.ErrorMessage });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
