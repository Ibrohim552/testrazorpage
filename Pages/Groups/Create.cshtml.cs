using Application.Contracts.Services;
using Application.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorApp.Pages.Groups
{
    public class CreateModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly ICourseService _courseService;

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public int CourseId { get; set; }

        [BindProperty]
        public int MaxStudents { get; set; }

        [BindProperty]
        public int MinStudents { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();

        public CreateModel(IGroupService groupService, ICourseService courseService)
        {
            _groupService = groupService;
            _courseService = courseService;
        }

        public async Task OnGetAsync()
        {
            // Получаем список курсов для отображения в форме
            var result = await _courseService.GetAll(new CourseFilter { PageNumber = 1, PageSize = 100 });
            if (result.IsSuccess)
            {
                Courses = result.Value!.Data!.ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var group = new Group
            {
                Name = Name,
                Description = Description,
                CourseId = CourseId,
                MaxStudents = MaxStudents,
                MinStudents = MinStudents,
                StartDate = StartDate,
                EndDate = EndDate
            };

            var result = await _groupService.Create(group);

            if (result.IsSuccess)
            {
                return RedirectToPage("/Groups/Index"); 
            }

            TempData["ErrorMessage"] = "Failed to create group!";
            return Page();
        }
    }
}
