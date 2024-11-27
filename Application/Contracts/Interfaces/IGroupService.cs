using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IGroupService
{
    Task<Result<bool>> Create(Group group);
    Task<Result<bool>> Update(int id, Group group);
    Task<Result<bool>> Delete(int id);
    Task<Result<Group>> GetById(int id);
    Task<Result<PagedResponse<IEnumerable<Group>>>> GetAll(GroupFilter filter);
}