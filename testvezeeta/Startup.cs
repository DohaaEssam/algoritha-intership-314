using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using testvezeeta.Application_Layer.Services.AdminServices;
using testvezeeta.Application_Layer.Services.DoctorServices;
using testvezeeta.Application_Layer.Services.PatientServices;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.IRepository;
using testvezeeta.Core_Layer.IServices.IAdminServices;
using testvezeeta.Core_Layer.IServices.IDoctorServices;
using testvezeeta.Core_Layer.IServices.IPatientServices;
using testvezeeta.Infrastructure_Layer;
using testvezeeta.Infrastructure_Layer.DbContext;
using testvezeeta.Infrastructure_Layer.Repoitory;

namespace testvezeeta
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<DbEntities>(optios =>
            //{
            //    optios.UseSqlServer("Data Source=.; Initial Catalog=DBs; Integrated Security = True");
            //});

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DbEntities>();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IPatientServices,PatientServices>();
            services.AddScoped<IAdminStatistics, AdminStatistics>();
            services.AddScoped< IDiscountCodeSetting, DiscountCodeSetting>();
            services.AddScoped<IDoctorInfo, DoctorInfo>();
            services.AddScoped<IPatientInfo, PatientInfo>();
            services.AddScoped<IDoctorServices, DoctorServices>();



            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "testvezeeta", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "testvezeeta v1"));
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                SeedDataService.Initialize(serviceProvider, userManager, roleManager).Wait();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

       
    }
}
