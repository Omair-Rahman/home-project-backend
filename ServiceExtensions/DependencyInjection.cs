using System.Text.Json;
using HomeProject.Database;
using HomeProject.Repositories.MediaContentRepository;
using HomeProject.Repositories.ProfileRepository;
using HomeProject.Services.CommonService;
using HomeProject.Services.MediaContentService;
using HomeProject.Services.ProfileService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeProject.ServiceExtensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services, IConfiguration config)
        {
            // Configure Entity Framework Core with PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidationFilterAttribute));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(p =>
                p.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
            services.AddHttpClient();
            services.AddScoped<ValidationFilterAttribute>();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            // Register services
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IMediaContentService, MediaContentService>();
            services.AddTransient<ICommonService, CommonService>();

            // Register repositories
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IMediaContentRepository, MediaContentRepository>();

            return services;
        }
    }
}
