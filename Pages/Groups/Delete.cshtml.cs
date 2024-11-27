using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.Contracts.Services;
using Domain.Entities;

namespace RazorApp.Pages.Groups;

public class DeleteModel : PageModel
{
    private readonly IGroupService _groupService;

    public DeleteModel(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public Group Group { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var groupResult = await _groupService.GetById(id);
        if (!groupResult.IsSuccess || groupResult.Value == null)
        {
            return NotFound();
        }

        Group = groupResult.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var deleteResult = await _groupService.Delete(id);
        if (!deleteResult.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Failed to delete group.");
            return Page();
        }

        return RedirectToPage("/Groups/Index");
    }
}
