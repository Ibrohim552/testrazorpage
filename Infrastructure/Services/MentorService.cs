using Application.Contracts.Services;
using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public class MentorService(DataContext context) : IMentorService
{
    public async Task<Result<bool>> Create(Mentor mentor)
    {
        if (mentor == null)
            return Result<bool>.Failure(Error.BadRequest());

        await context.AddAsync(mentor);
        int res = await context.SaveChangesAsync();

        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.BadRequest());
    }

    public async Task<Result<bool>> Delete(int id)
    {
        Mentor? mentor = await context.Mentors.FirstOrDefaultAsync(x => x.Id == id);
        if (mentor == null)
            return Result<bool>.Failure(Error.NotFound());
        mentor.IsDeleted = true;
        mentor.DeletedAt = DateTime.UtcNow;
        mentor.UpdatedAt = DateTime.UtcNow;
        mentor.Version += 1;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Update(int id, Mentor mentor)
    {
        Mentor? mentorUpdate = await context.Mentors.FirstOrDefaultAsync(x => x.Id == id);
        if (mentorUpdate == null)
            return Result<bool>.Failure(Error.NotFound());

        mentorUpdate.FirstName = mentor.FirstName;
        mentorUpdate.LastName = mentor.LastName;
        mentorUpdate.Email = mentor.Email;
        mentorUpdate.Expertise = mentor.Expertise;
        mentorUpdate.ExperienceInYears = mentor.ExperienceInYears;
        mentorUpdate.PhoneNumber = mentor.PhoneNumber;
        mentorUpdate.Gender = mentor.Gender;
        mentorUpdate.UpdatedAt = DateTime.UtcNow;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<Mentor>> GetById(int id)
    {
        Mentor? mentor = await context.Mentors.FirstOrDefaultAsync(x => x.Id == id);
        if (mentor is null)
            return Result<Mentor>.Failure(Error.NotFound());

        return Result<Mentor>.Success(mentor);
    }

    public async Task<Result<PagedResponse<IEnumerable<Mentor>>>> GetAll(MentorFilter filter)
    {
        IEnumerable<Mentor> mentors = context.Mentors.Where(x => x.IsDeleted == false);
        // if (filter.Name != null)
        //     mentors = mentors.Where(x => x.FirstName.Contains(filter.Name) || x.LastName.Contains(filter.Name));

        mentors = mentors.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

        int totalCount = await context.Mentors.CountAsync();
        PagedResponse<IEnumerable<Mentor>> response = PagedResponse<IEnumerable<Mentor>>
                .Create(filter.PageNumber, filter.PageSize, totalCount, mentors);
        return Result<PagedResponse<IEnumerable<Mentor>>>.Success(response);
    }
}