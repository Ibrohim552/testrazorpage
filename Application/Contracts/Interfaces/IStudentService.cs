using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
namespace Application.Contracts.Services;

public interface IStudentService
{
    Task<Result<bool>> Create(Student student);
    Task<Result<bool>> Update(int id, Student student);
    Task<Result<bool>> Delete(int id);
    Task<Result<Student>> GetById(int id);
    Task<Result<PagedResponse<IEnumerable<Student>>>> GetAll(StudentFilter filter);
}