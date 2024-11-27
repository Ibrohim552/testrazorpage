using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorApp.Pages.Groups
{
    public class EditModel : PageModel
    {
        private readonly IGroupService _groupService;

        public EditModel(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [BindProperty]
        public Domain.Entities.Group Group { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _groupService.GetById(Id);
            if (!result.IsSuccess)
            {
                return RedirectToPage("/NotFound");
            }

            Group = result.Value!;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _groupService.Update(Id, Group);
            if (result.IsSuccess)
            {
                return RedirectToPage("Details", new { id = Group.Id });
            }

            ModelState.AddModelError(string.Empty, "Error while updating group.");
            return Page();
        }
    }
}
