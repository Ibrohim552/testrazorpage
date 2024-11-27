using Application.Contracts.Services;
using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public class StudentGroupService(DataContext context) : IStudentGroupService
{
    public async Task<Result<bool>> Create(StudentGroup studentGroup)
    {
        if (studentGroup == null)
            return Result<bool>.Failure(Error.BadRequest());

        await context.AddAsync(studentGroup);
        int res = await context.SaveChangesAsync();

        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.BadRequest());
    }

    public async Task<Result<bool>> Delete(int id)
    {
        StudentGroup? studentGroup = await context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (studentGroup == null)
            return Result<bool>.Failure(Error.NotFound());
        studentGroup.IsDeleted = true;
        studentGroup.DeletedAt = DateTime.UtcNow;
        studentGroup.UpdatedAt = DateTime.UtcNow;
        studentGroup.Version += 1;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Update(int id, StudentGroup studentGroup)
    {
        StudentGroup? studentGroupUpdate = await context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (studentGroupUpdate == null)
            return Result<bool>.Failure(Error.NotFound());

        studentGroupUpdate.StudentId = studentGroup.StudentId;
        studentGroupUpdate.GroupId = studentGroup.GroupId;
        studentGroupUpdate.EnrolledAt = studentGroup.EnrolledAt;
        studentGroupUpdate.IsActive = studentGroup.IsActive;
        studentGroupUpdate.UpdatedAt = DateTime.UtcNow;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<StudentGroup>> GetById(int id)
    {
        StudentGroup? studentGroup = await context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (studentGroup is null)
            return Result<StudentGroup>.Failure(Error.NotFound());

        return Result<StudentGroup>.Success(studentGroup);
    }

    public async Task<Result<PagedResponse<IEnumerable<StudentGroup>>>> GetAll(StudentGroupFilter filter)
    {
        IEnumerable<StudentGroup> studentGroups = context.StudentGroups.Where(x => x.IsDeleted == false);
        // if (filter.StudentId != null)
        //     studentGroups = studentGroups.Where(x => x.StudentId == filter.StudentId);

        studentGroups = studentGroups.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

        int totalCount = await context.StudentGroups.CountAsync();
        PagedResponse<IEnumerable<StudentGroup>> response = PagedResponse<IEnumerable<StudentGroup>>
                .Create(filter.PageNumber, filter.PageSize, totalCount, studentGroups);
        return Result<PagedResponse<IEnumerable<StudentGroup>>>.Success(response);
    }
}
