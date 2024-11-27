using Application.Contracts.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<DataContext>(x => x.UseNpgsql(builder.Configuration["ConnectionString"]));
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ICourseService, CourseService>();
// builder.Services.AddScoped<ICourseService, CourseService>();
// builder.Services.AddScoped<ICourseService, CourseService>();
// builder.Services.AddScoped<ICourseService, CourseService>();
// builder.Services.AddScoped<ICourseService, CourseService>();

var app = builder.Build();



app.UseExceptionHandler("/Error");
app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();

app.Run();