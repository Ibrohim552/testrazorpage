using Application.Contracts.Services;
using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public class GroupService(DataContext context) : IGroupService
{
    public async Task<Result<bool>> Create(Group group)
    {
        if (group == null)
            return Result<bool>.Failure(Error.BadRequest());

        await context.AddAsync(group);
        int res = await context.SaveChangesAsync();

        return res > 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Delete(int id)
    {
        Group? group = await context.Groups.FirstOrDefaultAsync(x => x.Id == id);
        if (group == null)
            return Result<bool>.Failure(Error.NotFound());

        group.IsDeleted = true;
        group.DeletedAt = DateTime.UtcNow;
        group.UpdatedAt = DateTime.UtcNow;
        group.Version += 1;

        int res = await context.SaveChangesAsync();
        return res > 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Update(int id, Group group)
    {
        group.SetDatesToUtc();

        Group? groupUpdate = await context.Groups.FirstOrDefaultAsync(x => x.Id == id);
        if (groupUpdate == null)
            return Result<bool>.Failure(Error.NotFound());

        groupUpdate.Name = group.Name;
        groupUpdate.Description = group.Description;
        groupUpdate.CourseId = group.CourseId;
        groupUpdate.MaxStudents = group.MaxStudents;
        groupUpdate.MinStudents = group.MinStudents;
        groupUpdate.StartDate = group.StartDate;
        groupUpdate.EndDate = group.EndDate;
        groupUpdate.UpdatedAt = DateTime.UtcNow;
        groupUpdate.Version += 1;

        int res = await context.SaveChangesAsync();
        return res > 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<Group>> GetById(int id)
    {
        Group? group = await context.Groups
            .Include(x => x.Course)
            .Include(x => x.StudentGroups)
            .Include(x => x.MentorGroups)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (group == null)
            return Result<Group>.Failure(Error.NotFound());

        return Result<Group>.Success(group);
    }

    public async Task<Result<PagedResponse<IEnumerable<Group>>>> GetAll(GroupFilter filter)
    {
        IQueryable<Group> groups = context.Groups
            .Include(x => x.Course)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(filter.Name))
            groups = groups.Where(x => x.Name.Contains(filter.Name));

        int totalCount = await groups.CountAsync();

        groups = groups
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize);

        var response = PagedResponse<IEnumerable<Group>>.Create(
            filter.PageNumber,
            filter.PageSize,
            totalCount,
            await groups.ToListAsync()
        );

        return Result<PagedResponse<IEnumerable<Group>>>.Success(response);
    }
}
