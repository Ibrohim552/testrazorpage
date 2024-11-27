using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IStudentGroupService
{
    Task<Result<bool>> Create(StudentGroup studentGroup);
    Task<Result<bool>> Update(int id, StudentGroup studentGroup);
    Task<Result<bool>> Delete(int id);
    Task<Result<StudentGroup>> GetById(int id);
    Task<Result<PagedResponse<IEnumerable<StudentGroup>>>> GetAll(StudentGroupFilter filter);
}