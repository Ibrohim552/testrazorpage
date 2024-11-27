using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorApp.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly IGroupService _groupService;

        public DetailsModel(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Domain.Entities.Group Group { get; set; } = null!;

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
    }
}
