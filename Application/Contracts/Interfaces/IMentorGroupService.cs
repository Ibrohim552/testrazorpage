using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IMentorGroupService
{
    Task<Result<bool>> Create(MentorGroup mentorGroup);
    Task<Result<bool>> Update(int id, MentorGroup mentorGroup);
    Task<Result<bool>> Delete(int id);
    Task<Result<MentorGroup>> GetById(int id);
    Task<Result<PagedResponse<IEnumerable<MentorGroup>>>> GetAll(MentorGroupFilter filter);
}