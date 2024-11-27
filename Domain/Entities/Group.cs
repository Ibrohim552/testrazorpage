using Domain.Common;

namespace Domain.Entities;

public class Group : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public int MaxStudents { get; set; }
    public int MinStudents { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Course? Course { get; set; }
    public ICollection<StudentGroup> StudentGroups { get; set; } = [];
    public ICollection<MentorGroup> MentorGroups { get; set; } = [];

    public void SetDatesToUtc()
    {
        StartDate = StartDate.ToUniversalTime();  // Преобразуем StartDate в UTC
        EndDate = EndDate.ToUniversalTime();      // Преобразуем EndDate в UTC
    }
}