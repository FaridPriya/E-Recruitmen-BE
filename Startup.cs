using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
            SetJwt(services);

            services.AddHealthChecks();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E- Recruitmen", Version = "v1" });

                // Konfigurasi autentikasi Bearer Token
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Masukkan token Bearer JWT di sini",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                };
                c.AddSecurityRequirement(securityRequirement);

                // Ini adalah contoh jika Anda ingin menggunakan atribut Authorize di action controller
                // c.EnableAnnotations();
            });
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

            app.UseAuthentication();

            app.UseAuthorization();

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
                option.RoutePrefix = "docs/v1";
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

        private static void SetJwt(IServiceCollection services)
        {
            var jwtKey = Encoding.ASCII.GetBytes("FaridGantengSekali881123");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
                    };
                });
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
