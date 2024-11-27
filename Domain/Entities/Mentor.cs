using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Mentor : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ExpertiseType Expertise { get; set; }
    public int ExperienceInYears { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public ICollection<MentorGroup> MentorGroups { get; set; } = [];
}