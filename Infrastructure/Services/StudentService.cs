using Application.Contracts.Services;
using Application.Responses;
using Application.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

public class StudentService(DataContext context) : IStudentService
{
    public async Task<Result<bool>> Create(Student student)
    {
        if (student == null)
            return Result<bool>.Failure(Error.BadRequest());

        await context.AddAsync(student);
        int res = await context.SaveChangesAsync();

        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.BadRequest());
    }

    public async Task<Result<bool>> Delete(int id)
    {
        Student? student = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (student == null)
            return Result<bool>.Failure(Error.NotFound());
        student.IsDeleted = true;
        student.DeletedAt = DateTime.UtcNow;
        student.UpdatedAt = DateTime.UtcNow;
        student.Version += 1;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> Update(int id, Student student)
    {
        Student? studentUpdate = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (studentUpdate == null)
            return Result<bool>.Failure(Error.NotFound());

        studentUpdate.FirstName = student.FirstName;
        studentUpdate.LastName = student.LastName;
        studentUpdate.Email = student.Email;
        studentUpdate.BirthDate = student.BirthDate;
        studentUpdate.PhoneNumber = student.PhoneNumber;
        studentUpdate.Gender = student.Gender;
        studentUpdate.UpdatedAt = DateTime.UtcNow;
        int res = await context.SaveChangesAsync();
        return res > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<Student>> GetById(int id)
    {
        Student? student = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (student is null)
            return Result<Student>.Failure(Error.NotFound());

        return Result<Student>.Success(student);
    }

    public async Task<Result<PagedResponse<IEnumerable<Student>>>> GetAll(StudentFilter filter)
    {
        IEnumerable<Student> students = context.Students.Where(x => x.IsDeleted == false);
        // if (filter.Name != null)
        //     students = students.Where(x => x.FirstName.Contains(filter.Name) || x.LastName.Contains(filter.Name));

        students = students.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

        int totalCount = await context.Students.CountAsync();
        PagedResponse<IEnumerable<Student>> response = PagedResponse<IEnumerable<Student>>
                .Create(filter.PageNumber, filter.PageSize, totalCount, students);
        return Result<PagedResponse<IEnumerable<Student>>>.Success(response);
    }
}
