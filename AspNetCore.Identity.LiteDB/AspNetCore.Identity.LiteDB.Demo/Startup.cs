﻿using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Demo.Services;
using AspNetCore.Identity.LiteDB.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCore.Identity.LiteDB.Demo
{
   public class Startup
   {
      public IConfiguration Configuration { get; }

      public Startup(IConfiguration configuration) => Configuration = configuration;

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         // Add LiteDB Dependency. Thare are three ways to set database:
         // 1. By default it uses the first connection string on appsettings.json, ConnectionStrings section.
         services.AddSingleton<ILiteDbContext, LiteDbContext>();

         // 2. Custom context implementing ILiteDbContext
         //services.AddSingleton<AppDbContext>();

         // 3. Cusom context by using constructor
         //services.AddSingleton<ILiteDbContext, LiteDbContext>(x => new LiteDbContext(new LiteDatabase("Filename=Database.db")));

         services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
               options.Password.RequireDigit = false;
               options.Password.RequireUppercase = false;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequiredLength = 6;
            })
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<LiteDbUserStore<ApplicationUser>>()
            .AddRoleStore<LiteDbRoleStore<IdentityRole>>()
            .AddDefaultTokenProviders();

         // Add application services.
         services.AddTransient<IEmailSender, EmailSender>();

         services.AddMvc();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
         }

         app.UseStaticFiles();

         app.UseRouting();

         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapDefaultControllerRoute();
         });
      }
   }
}
