using Autofac;
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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Commerce.Amazon.Engine.Managers;
using Commerce.Amazon.Web.Repositories;
using Commerce.Amazon.Web.Managers.Interfaces;

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


            services.AddEntityFrameworkNpgsql().AddDbContext<MyContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DataCommerceConnection")));


            builder.RegisterType<OperationManager>().As<IOperationManager>();
            builder.RegisterType<HostingEnvironment>().As<IHostingEnvironment>();
            builder.RegisterType<CustomSiteMapModule>();

            //services.AddSingleton<ILoggerManager, LoggerManager>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            builder.Populate(services);

            var container = builder.Build();

            //var operationManager = container.Resolve<IOperationManager>();
            //try
            //{
            //    operationManager.SaveUser(new User
            //    {
            //        Id = 1,
            //        Nom = "DDAD",
            //        Prenom = "Abdou",
            //        Email = "abdouhdd@outlook.com",
            //        UserId = "ABDOU",
            //        UserGuid = "ABDOU1234",
            //        State = 1
            //    });
            //}
            //catch (Exception ex)
            //{
            //    Console.Error.WriteLine(ex);
            //    throw;
            //}

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
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
