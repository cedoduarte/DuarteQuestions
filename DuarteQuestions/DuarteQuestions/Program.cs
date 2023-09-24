using AutoMapper;
using DuarteQuestions.Model;
using DuarteQuestions.Model.Interface;
using DuarteQuestions.Service;
using DuarteQuestions.Service.Interface;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DuarteQuestions
{
    public class Program
    {
        private const string ConnectionStringName = "DuarteQuestions";
        private const string ApiVersion = "v1";
        private const string ApiTitle = "DuarteQuestions";
        private const string ApiDescription = "DuarteQuestions .NET Core Web API";
        private const string ApiTermsOfServiceUrl = "https://www.example.com/terms";
        private const string ApiContactName = "Carlos Enrique Duarte Ortiz";
        private const string ApiContactUrl = "https://www.duartecorporation.com";
        private const string ApiLicense = "Free License";
        private const string ApiLicenseUrl = "https://www.example.com/license";
        private const string ApiSwaggerEndpoint = "/swagger/v1/swagger.json";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<IConfiguration>(a =>
            {
                string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.Local.json", optional: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                    .AddEnvironmentVariables();
                return configBuilder.Build();
            });

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            builder.Services.Configure<FormOptions>(options => 
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiVersion, new OpenApiInfo()
                {
                    Version = ApiVersion,
                    Title = ApiTitle,
                    Description = ApiDescription,
                    TermsOfService = new Uri(ApiTermsOfServiceUrl),
                    Contact = new OpenApiContact() 
                    {
                        Name = ApiContactName,
                        Url = new Uri(ApiContactUrl)
                    },
                    License = new OpenApiLicense()
                    {
                        Name = ApiLicense,
                        Url = new Uri(ApiLicenseUrl)
                    }
                });
            });

            builder.Services.AddTransient<IAppDbContext, AppDbContext>();
            builder.Services.AddTransient<IAnswerService, AnswerService>();
            builder.Services.AddTransient<IQuestionService, QuestionService>();
            builder.Services.AddTransient<IUserService, UserService>();

            // Mapper
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

            string? connectionString = builder.Configuration.GetConnectionString(ConnectionStringName);
            builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            builder.Services.AddMvc();

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                try
                {
                    AppDbContext? dbContext = (AppDbContext?)scope.ServiceProvider.GetService<IAppDbContext>();
                    dbContext?.Database.Migrate();
                    DbSeeder.DoSeeding(dbContext);
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }

            // Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(ApiSwaggerEndpoint, ApiVersion);
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}