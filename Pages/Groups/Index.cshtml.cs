using Application.Contracts.Services;
using Application.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorApp.Pages.Groups;

public class IndexModel : PageModel
{
    private readonly IGroupService _groupService;

    public IndexModel(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public PagedResponse<IEnumerable<Group>> Groups { get; set; } = default!;

    public async Task OnGetAsync([FromQuery] GroupFilter filter)
    {
        var result = await _groupService.GetAll(filter);
        if (result.IsSuccess)
        {
            Groups = result.Value!;
        }
    }
}
