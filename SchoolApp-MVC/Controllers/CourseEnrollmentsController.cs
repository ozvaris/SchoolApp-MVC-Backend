using Microsoft.AspNetCore.Mvc;
using SchoolApp_MVC.ApiClients.Interfaces;
using SchoolApp_MVC.Dtos.CourseEnrollments;
using SchoolApp_MVC.Dtos.Students;
using SchoolApp_MVC.ViewModels.CourseEnrollments;

namespace SchoolApp_MVC.Controllers;

public class CourseEnrollmentsController : Controller
{
    private readonly ICourseEnrollmentApiClient _courseEnrollmentApiClient;
    private readonly ICourseApiClient _courseApiClient;

    public CourseEnrollmentsController(
        ICourseEnrollmentApiClient courseEnrollmentApiClient,
        ICourseApiClient courseApiClient)
    {
        _courseEnrollmentApiClient = courseEnrollmentApiClient;
        _courseApiClient = courseApiClient;
    }

    public async Task<IActionResult> Index(int? courseId)
    {
        var model = await BuildIndexViewModelAsync(courseId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseEnrollmentCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please select a course and student.";
            return RedirectToAction(nameof(Index), new { courseId = dto.CourseID });
        }

        var result = await _courseEnrollmentApiClient.CreateAsync(dto);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage ?? "Create operation failed.";
            return RedirectToAction(nameof(Index), new { courseId = dto.CourseID });
        }

        TempData["SuccessMessage"] = "Course enrollment created successfully.";
        return RedirectToAction(nameof(Index), new { courseId = dto.CourseID });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var enrollment = await _courseEnrollmentApiClient.GetByIdAsync(id);
        if (enrollment is null)
        {
            return NotFound();
        }

        SetEnrollmentViewData(enrollment);

        var model = new CourseEnrollmentUpdateDto
        {
            CourseEnrollmentID = enrollment.CourseEnrollmentID,
            EnrollmentDate = enrollment.EnrollmentDate
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CourseEnrollmentUpdateDto dto)
    {
        if (id != dto.CourseEnrollmentID)
        {
            return BadRequest();
        }

        var enrollment = await _courseEnrollmentApiClient.GetByIdAsync(id);
        if (enrollment is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            SetEnrollmentViewData(enrollment);
            return View(dto);
        }

        var result = await _courseEnrollmentApiClient.UpdateAsync(id, dto);
        if (!result.Success)
        {
            SetEnrollmentViewData(enrollment);
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Update operation failed.");
            return View(dto);
        }

        TempData["SuccessMessage"] = "Course enrollment updated successfully.";
        return RedirectToAction(nameof(Index), new { courseId = enrollment.CourseID });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var enrollment = await _courseEnrollmentApiClient.GetByIdAsync(id);
        if (enrollment is null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _courseEnrollmentApiClient.GetByIdAsync(id);
        if (enrollment is null)
        {
            return NotFound();
        }

        var deleted = await _courseEnrollmentApiClient.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Course enrollment deleted successfully.";
        return RedirectToAction(nameof(Index), new { courseId = enrollment.CourseID });
    }

    private async Task<CourseEnrollmentIndexViewModel> BuildIndexViewModelAsync(int? courseId)
    {
        var courses = await _courseApiClient.GetAllAsync();
        var selectedCourseId = courseId ?? courses.FirstOrDefault()?.CourseID;

        IReadOnlyList<CourseEnrollmentReadDto> enrollments = selectedCourseId.HasValue
            ? await _courseEnrollmentApiClient.GetByCourseIdAsync(selectedCourseId.Value)
            : [];

        IReadOnlyList<StudentReadDto> availableStudents = selectedCourseId.HasValue
            ? await _courseEnrollmentApiClient.GetAvailableStudentsByCourseIdAsync(selectedCourseId.Value)
            : [];

        return new CourseEnrollmentIndexViewModel
        {
            Courses = courses,
            Enrollments = enrollments,
            AvailableStudents = availableStudents,
            SelectedCourseID = selectedCourseId,
            NewEnrollment = new CourseEnrollmentCreateDto
            {
                CourseID = selectedCourseId ?? 0,
                EnrollmentDate = DateTime.Today
            }
        };
    }

    private void SetEnrollmentViewData(CourseEnrollmentReadDto enrollment)
    {
        ViewData["CourseID"] = enrollment.CourseID;
        ViewData["CourseName"] = $"{enrollment.CourseCode} - {enrollment.CourseName}";
        ViewData["StudentName"] = $"{enrollment.StudentName} {enrollment.StudentSurname}";
        ViewData["StudentEmail"] = enrollment.StudentEmail;
    }
}
