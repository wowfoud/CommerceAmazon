﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Commerce.Amazon.Web.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Commerce.Amazon.Web.Repositories;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Tools.Tools;
using Commerce.Amazon.Tools.Contracts;
using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Web.Managers;

namespace Commerce.Amazon.Web
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddHttpContextAccessor();

            var builder = new Autofac.ContainerBuilder();

            GlobalConfiguration.Setting = Configuration.GetSection("Settings").Get<Settings>();
            GlobalConfiguration.Setting.DataCommerceConnectionLocal = Configuration.GetConnectionString(nameof(GlobalConfiguration.Setting.DataCommerceConnectionLocal));
            GlobalConfiguration.Setting.DataCommerceConnectionServer = Configuration.GetConnectionString(nameof(GlobalConfiguration.Setting.DataCommerceConnectionServer));
            services.AddEntityFrameworkNpgsql().AddDbContext<MyContext>(opt => opt.UseNpgsql(GlobalConfiguration.Setting.DataCommerceConnectionLocal));

            //services.AddScoped<IOperationManager, OperationManager>();
            //services.AddScoped<IAccountManager, AccountManager>();
            //services.AddScoped<IMailSender, MailSender>();
            //services.AddScoped<IHostingEnvironment, HostingEnvironment>();
            //services.AddScoped<CustomSiteMapModule>();

            //builder.RegisterType<UserManager>().As<IUserManager>();
            //builder.RegisterType<AccountManager>().As<IAccountManager>();
            //builder.RegisterType<MailSender>().As<IMailSender>();
            builder.RegisterType<HostingEnvironment>().As<IHostingEnvironment>();
            builder.RegisterType<CustomSiteMapModule>();

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IAdminManager, AdminManager>();
            services.AddScoped<IMailSender, MailSender>();
            services.AddScoped<AccountProcess>();
            services.AddScoped<UserProcess>();
            services.AddScoped<AdminProcess>();
            services.AddScoped<TokenManager>();
            services.AddScoped<TestProcess>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            builder.Populate(services);

            GlobalConfiguration.Messages = new Messages();
            var container = builder.Build();

            var testProcess = container.Resolve<TestProcess>();
            var accountManager = container.Resolve<IAccountManager>();
            try
            {
                //testProcess.InitDatabase();
                //accountManager.Authenticate(new Domain.Models.Request.Auth.AuthenticationRequest
                //{
                //    Email = "abderrahmanhdd@gmail.com",
                //    Password = "123456"
                //});
                //testProcess.AddGroups();
                //testProcess.AddUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return container.Resolve<IServiceProvider>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePages(ctx =>
            //{
            //    if (ctx.HttpContext.Response.StatusCode == 405)
            //        ctx.HttpContext.Response.StatusCode = 404;

            //    return Task.CompletedTask;
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Post}/{action=NewPost}/{id?}");
            });
        }
    }
}
