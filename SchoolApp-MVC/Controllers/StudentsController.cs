using Microsoft.AspNetCore.Mvc;
using SchoolApp_MVC.ApiClients.Interfaces;
using SchoolApp_MVC.Dtos.Students;

namespace SchoolApp_MVC.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentApiClient _studentApiClient;

    public StudentsController(IStudentApiClient studentApiClient)
    {
        _studentApiClient = studentApiClient;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentApiClient.GetAllAsync();
        return View(students);
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentApiClient.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    public IActionResult Create()
    {
        return View(new StudentCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _studentApiClient.CreateAsync(dto);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Create operation failed.");
            return View(dto);
        }

        TempData["SuccessMessage"] = "Student created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentApiClient.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        var model = new StudentUpdateDto
        {
            StudentID = student.StudentID,
            StudentName = student.StudentName,
            StudentSurname = student.StudentSurname,
            StudentEmail = student.StudentEmail
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentUpdateDto dto)
    {
        if (id != dto.StudentID)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _studentApiClient.UpdateAsync(id, dto);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Update operation failed.");
            return View(dto);
        }

        TempData["SuccessMessage"] = "Student updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var student = await _studentApiClient.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _studentApiClient.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Student deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
