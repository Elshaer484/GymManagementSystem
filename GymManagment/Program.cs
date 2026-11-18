using GymManagment.DAL.Repositorities.Classes;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GymManagment.DAL.Data.DbContexts;
using GymManagment.BLL.ViewModels.Classes;
using GymManagment.BLL.Classes.Interfaces;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.Classes;
using GymManagment.BLL;
using GymManagment.DAL.Data.DataSeeding;
using GymManagment.PL;
using GymManagment.BLL.Attachments;
using GymManagment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GymManagment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ImemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IDashBoardService, DashBoardService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                conf.Lockout.MaxFailedAccessAttempts = 5;

            }).AddEntityFrameworkStores<GymDbContext>();
            
            var app = builder.Build();


            await app.MigrateAndSeedData();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
