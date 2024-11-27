using Domain.Common;

namespace Domain.Entities;

public class StudentGroup : BaseEntity
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Student? Student { get; set; }
    public Group? Group { get; set; }
}