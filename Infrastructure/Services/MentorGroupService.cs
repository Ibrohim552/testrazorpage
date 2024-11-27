using Application.Contracts.Services;
using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public class MentorGroupService(DataContext context) : IMentorGroupService
{
    public async Task<Result<bool>> Create(MentorGroup mentorGroup)
    {
        if (mentorGroup == null)
            return Result<bool>.Failure(Error.BadRequest());

        await context.AddAsync(mentorGroup);
        int res = await context.SaveChangesAsync();

        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.BadRequest());
    }

    public async Task<Result<bool>> Delete(int id)
    {
        MentorGroup? mentorGroup = await context.MentorGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (mentorGroup == null)
            return Result<bool>.Failure(Error.NotFound());
        mentorGroup.IsDeleted = true;
        mentorGroup.DeletedAt = DateTime.UtcNow;
        mentorGroup.UpdatedAt = DateTime.UtcNow;
        mentorGroup.Version += 1;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Update(int id, MentorGroup mentorGroup)
    {
        MentorGroup? mentorGroupUpdate = await context.MentorGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (mentorGroupUpdate == null)
            return Result<bool>.Failure(Error.NotFound());

        mentorGroupUpdate.MentorId = mentorGroup.MentorId;
        mentorGroupUpdate.GroupId = mentorGroup.GroupId;
        mentorGroupUpdate.RoleType = mentorGroup.RoleType;
        mentorGroupUpdate.UpdatedAt = DateTime.UtcNow;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<MentorGroup>> GetById(int id)
    {
        MentorGroup? mentorGroup = await context.MentorGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (mentorGroup is null)
            return Result<MentorGroup>.Failure(Error.NotFound());

        return Result<MentorGroup>.Success(mentorGroup);
    }

    public async Task<Result<PagedResponse<IEnumerable<MentorGroup>>>> GetAll(MentorGroupFilter filter)
    {
        IEnumerable<MentorGroup> mentorGroups = context.MentorGroups.Where(x => x.IsDeleted == false);
        // if (filter.MentorId != null)
        //     mentorGroups = mentorGroups.Where(x => x.MentorId == filter.MentorId);

        mentorGroups = mentorGroups.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

        int totalCount = await context.MentorGroups.CountAsync();
        PagedResponse<IEnumerable<MentorGroup>> response = PagedResponse<IEnumerable<MentorGroup>>
                .Create(filter.PageNumber, filter.PageSize, totalCount, mentorGroups);
        return Result<PagedResponse<IEnumerable<MentorGroup>>>.Success(response);
    }
}
