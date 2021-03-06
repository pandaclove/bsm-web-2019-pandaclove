using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ShowApp.Data.Abstract;
using ShowApp.Data.Concrete.EfCore;
using ShowApp.Data.Concrete.EfCore.Identity;

namespace ShowApp.WebUI
{
	public class Startup
	{
		public IConfiguration configuration { get; }
		public Startup(IConfiguration _configuration)
		{
			configuration = _configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<ICategoryRepository, EfCategoryRepository>();
			services.AddTransient<IGenreRepository, EfGenreRepository>();
			services.AddTransient<IShowRepository, EfShowRepository>();
			services.AddDbContext<ApplicationDbContext>();
			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			services.Configure<IdentityOptions>(options =>
				{
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = true;
					options.Password.RequireUppercase = false;
					options.Password.RequiredLength = 8;
					options.Password.RequireNonAlphanumeric = false;

					options.Lockout.MaxFailedAccessAttempts = 5;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
					options.User.RequireUniqueEmail = true;
				}
			);
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
				Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
				RequestPath = "/modules"
			});//node_modules
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseStatusCodePages();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
						name: "AdminRoute",
						pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);
				endpoints.MapControllerRoute(
						name: "DefaultRoute",
						pattern: "{controller=Home}/{action=Index}/{id?}"
					);

			});
		}
	}
}
