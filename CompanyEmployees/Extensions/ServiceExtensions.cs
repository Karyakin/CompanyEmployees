using CompanyEmployees.Helpers;
using Conrtacts;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// мы собираемся сделать, это настроить CORS в нашем приложении. CORS (Cross-Origin Resource Sharing) - это механизм для предоставления или ограничения прав доступа к приложениям из разных доменов.
        ///this представляет тип данных объекта, который будет использовать этот метод расширения.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin() // Вместо AllowAnyOrigin () метод, который разрешает запросы из любого источника, мы можем использовать WithOrigins например, WithOrigins("https://example.com")
            .AllowAnyMethod()// вместо AllowAnyMethod () что позволяет использовать все методы HTTP, мы можем использовать WithMethods ("POST", "GET") это позволит использовать только определенные методы HTTP
            .AllowAnyHeader());// WithHeaders ("accept", "content- type") , чтобы разрешить только определенные заголовки.
        });

        /// <summary>
        /// Приложения ASP.NET Core по умолчанию размещаются самостоятельно, и если мы хотим разместить наше приложение в IIS, нам необходимо настроить интеграцию IIS, которая в конечном итоге поможет нам с развертыванием в IIS. 
        /// Хостирование приложений ASP.NET Core на IIS происходит с помощью нативного модуля AspNetCoreModule, который сконфигурирован таким образом, чтобы перенаправлять запросы на веб-сервер Kestrel. Этот модуль управляет запуском внешнего процесса dotnet.exe, в рамках которого хостируется приложение, и перенаправляет все запросы от IIS к этому хостирующему процессу.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services) => services
            .Configure<IISOptions>(options =>
        {
        });

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();


        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) =>
                services.AddDbContext<RepositoryContext>(opts =>
                    opts.UseSqlServer(configuration.GetConnectionString("SqlConnectionStr"), b =>
                        b.MigrationsAssembly("CompanyEmployees")));
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) => 
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));



    }
}
