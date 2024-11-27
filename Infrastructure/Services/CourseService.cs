using Application.Contracts.Services;
using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public class CourseService(DataContext context) : ICourseService
{
    public async Task<Result<bool>> Create(Course course)
    {
        if (course == null)
            return Result<bool>.Failure(Error.BadRequest());

        await context.AddAsync(course);
        int res = await context.SaveChangesAsync();

        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.BadRequest());
    }

    public async Task<Result<bool>> Delete(int id)
    {
        Course? course = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (course == null)
            return Result<bool>.Failure(Error.NotFound());
        course.IsDeleted = true;
        course.DeletedAt = DateTime.UtcNow;
        course.UpdatedAt = DateTime.UtcNow;
        course.Version += 1;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Update(int id, Course course)
    {
        Course? courseUpdate = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (courseUpdate == null)
            return Result<bool>.Failure(Error.NotFound());

        courseUpdate!.Name = course.Name;
        courseUpdate.Description = course.Description;
        courseUpdate.Price = course.Price;
        courseUpdate.Duration = course.Duration;
        courseUpdate.DurationType = course.DurationType;
        courseUpdate.UpdatedAt = DateTime.UtcNow;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<Course>> GetById(int id)
    {
        Course? course = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (course is null)
            return Result<Course>.Failure(Error.NotFound());

        return Result<Course>.Success(course);
    }

    public async Task<Result<PagedResponse<IEnumerable<Course>>>> GetAll(CourseFilter filter)
    {
        IEnumerable<Course> courses = context.Courses.Where(x => x.IsDeleted == false);
        // if (filter.Name != null)
        //     courses = courses.Where(x => x.Name == filter.Name);

        courses = courses.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

        int totalCount = await context.Courses.Where(x => x.IsDeleted == false).CountAsync();
        PagedResponse<IEnumerable<Course>> response = PagedResponse<IEnumerable<Course>>
                .Create(filter.PageNumber, filter.PageSize, totalCount, courses);
        return Result<PagedResponse<IEnumerable<Course>>>.Success(response);
    }
}