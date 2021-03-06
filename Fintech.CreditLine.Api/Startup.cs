using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Fintech.CreditLine.Api.Filters;
using Fintech.CreditLine.Api.Middlewares;
using Fintech.CreditLine.Data;
using Fintech.CreditLine.Data.Repositories;
using Fintech.CreditLineRequests.Domain.CreditLines.Repositories;
using Fintech.CreditLineRequests.Domain.CreditLines.Services;
using Fintech.CreditLineRequests.Services;
using Fintech.CreditLine.Api.Services;

namespace Fintech.CreditLineRequests.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            })
            .AddFluentValidation(x => 
            {
                x.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fintech Credit Line API", Version = "v1" });
            });

            services.AddTransient<ICreditLineRequestRepository, CreditLineRequestRepository>();
            services.AddTransient<ICreditLineRequestService, CreditLineRequestService>();

            services.AddDbContext<DataContext>(options =>
            {
                string connectionStr = _configuration.GetConnectionString("DefaultConnection");

                options.UseNpgsql(connectionStr);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fintech Credit Line API"));

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthorization();

            InitialMigration.MigrationInitialisation(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
