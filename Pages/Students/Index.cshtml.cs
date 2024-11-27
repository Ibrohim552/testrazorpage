using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorApp.Pages.Students;

public class IndexModel : PageModel
{
    private readonly DataContext _context;

    public IndexModel(DataContext context)
    {
        _context = context;
    }

    public IList<Student> Students { get; set; }

    public async Task OnGetAsync()
    {
        Students = await _context.Students.ToListAsync();
    }
}
