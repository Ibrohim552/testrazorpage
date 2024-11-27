using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public CourseDurationType DurationType { get; set; } = CourseDurationType.Month;
    public ICollection<Group> Groups { get; set; } = [];
}