using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace ERecruitmentBE
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(SetJsonSerializerOptions);

            services.AddEntityFrameworkSqlServer();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            SetDatabaseContext(services);
            SetAutoMapper(services);

            services.AddHealthChecks();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Add Health Checks Endpoint
                endpoints.MapHealthChecks("/api/values");
            });

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                option.RoutePrefix = String.Empty;
                option.DocumentTitle = "E-Recruitmen Engine";
            });
        }

        protected virtual void SetDatabaseContext(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), opt =>
                    opt.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)));
        }

        private static void SetAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GlobalMapping());
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static void SetJsonSerializerOptions(JsonOptions jsonOptions)
        {
            jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            // keeps the casing to that of the model when serializing to json
            // (default is converting to camelCase)
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
        }
    }
}
