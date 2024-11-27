using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IMentorService
{
    Task<Result<bool>> Create(Mentor mentor);
    Task<Result<bool>> Update(int id, Mentor mentor);
    Task<Result<bool>> Delete(int id);
    Task<Result<Mentor>> GetById(int id);
    Task<Result<PagedResponse<IEnumerable<Mentor>>>> GetAll(MentorFilter filter);
}