using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class MentorGroup : BaseEntity
{
    public int MentorId { get; set; }
    public int GroupId { get; set; }
    public MentorRoleType RoleType { get; set; }
    public Mentor? Mentor { get; set; }
    public Group? Group { get; set; }
}