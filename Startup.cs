using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using webapp.Models;
using webapp.Security;
using Microsoft.AspNetCore.Authentication;

namespace webapp
{
    public class Startup
    {
        private IConfiguration _conf;

        public Startup(IConfiguration config)
        {
            _conf = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_conf.GetConnectionString("EmployeeDBConnection")));


            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 10;
            //    options.Password.RequiredUniqueChars = 3;

            //    options.SignIn.RequireConfirmedEmail = true;
            //    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            //});



            //services.AddIdentity<IdentityUser, IdentityRole>()
            //        .AddEntityFrameworkStores<AppDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;

                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                //user lock out for password attempt failure
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            services.Configure<DataProtectionTokenProviderOptions>(o => 
                    o.TokenLifespan = TimeSpan.FromHours(5));
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                   o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "26869655549-9on1hv6gf6vpahpeqog9jo655ck18ssl.apps.googleusercontent.com";
                options.ClientSecret = "MQN0uwKUEnHOf_2NYK89nBFa";


            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");

            });

          

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role")
                                                                      .RequireClaim("Edit Role"));

                //custom policy

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => 
                //                                      context.User.IsInRole("Admin") &&  
                //                                      context.User.HasClaim(claim => claim.Type=="Edit Role" && claim.Value=="true")||
                //                                      context.User.IsInRole("Super Admin")
                //                                          ));

                options.AddPolicy("EditRolePolicy",
                            policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

              //  options.InvokeHandlersAfterFailure = false;

                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));

               // options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));

            });

            //services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);
           // services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            //register custom edit policy
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>(); 

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
                //  app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");


            }


            //DefaultFilesOptions defaultfileoption = new DefaultFilesOptions();
            //defaultfileoption.DefaultFileNames.Clear();
            //defaultfileoption.DefaultFileNames.Add("foo.html");
            //app.UseDefaultFiles(defaultfileoption);
            //app.UseStaticFiles();

            //FileServerOptions defaultfileoption = new FileServerOptions();
            //defaultfileoption.DefaultFilesOptions.DefaultFileNames.Clear();
            //defaultfileoption.DefaultFilesOptions.DefaultFileNames.Add("foo.html");

            //app.UseFileServer(defaultfileoption);

            //  app.UseFileServer();
            app.UseStaticFiles();
            //   app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //   // throw new Exception("something wrong happened");
            //    await context.Response.WriteAsync("hello world");
            //});

            

           //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync(_conf["Mykey"]);
            //    });
            //});
        }
    }
}
