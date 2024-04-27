using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Route.C41.G02.DAL.Data;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.Extensions;
using Route.C41.G02.PL.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Route.C41.G02.PL
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services

			webApplicationBuilder.Services.AddControllersWithViews();

			
			webApplicationBuilder.Services.AddDbContext<ApplicationDbcontext>(options =>

			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			}); 



			webApplicationBuilder.Services.AddAuthentication(options =>
			{

			})
				.AddCookie("Hamada", options =>
				{
					options.LoginPath = "/Account/SignIn";
					options.AccessDeniedPath = "/Home/Error";
					options.ExpireTimeSpan = TimeSpan.FromDays(1); // expire after day
				}
				);


			webApplicationBuilder.Services.AddApplicationServices();
			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));



			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 5;
				options.Password.RequiredUniqueChars = 2;
				options.Password.RequireNonAlphanumeric = true;

				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5; 
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

				options.User.RequireUniqueEmail = true; 
			}
			).AddEntityFrameworkStores<ApplicationDbcontext>()// allow dependency injection for user store
			.AddDefaultTokenProviders(); // allow token to reset password

			webApplicationBuilder.Services.ConfigureApplicationCookie(options => {
				options.LoginPath = "/Account/SignIn";
				options.AccessDeniedPath = "/Home/Error";
				options.ExpireTimeSpan = TimeSpan.FromDays(1); // expire after day
			});


			#endregion


			var app= webApplicationBuilder.Build();
			#region Configure Kestrel Middlewares


			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting(); 

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			#endregion


			app.Run();

		}


	}
}
