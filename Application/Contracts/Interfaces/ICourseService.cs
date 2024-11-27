using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface ICourseService
{
    Task<Result<bool>> Create(Course course);
    Task<Result<bool>> Update(int id, Course course);
    Task<Result<bool>> Delete(int id);
    Task<Result<Course>> GetById(int id);
    Task<Result<PagedResponse<IEnumerable<Course>>>> GetAll(CourseFilter filter);
}