using AutoMapper;
using Data;
using Data.Configurations;
using Data.Utilities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace API
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">services instance</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString(Constants.AppConfig.ConnectionString),
                    b => b.MigrationsAssembly(Constants.AppConfig.MigrationAssembly)));

            //for login
            ConfigureAuth(services, Configuration.GetValue<string>("SecureKey"));
            //inject dependencies
            InjectDependencies(services);
            //configure auto mapper
            ConfigureMapper(services);
            //configure swagger
            ConfigureSwagger(services);

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = CustomContractResolver.Instance;
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">application instance</param>
        /// <param name="env">hosting environment instance</param>
        /// <param name="loggerFactory">for logging</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();

            //use swagger ui
            UseSwagger(app);

            app.UseSerilogRequestLogging(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedDatabase.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
        }
    }
}