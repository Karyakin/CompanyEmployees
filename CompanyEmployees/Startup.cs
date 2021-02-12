using AutoMapper;
using CompanyEmployees.Filters;
using CompanyEmployees.Extensions;
using CompanyEmployees.Helpers;
using Conrtacts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System.IO;
using CompanyEmployees.Filters.TestsFlters;

namespace CompanyEmployees
{
    /// <summary>
    /// В классе Startup мы настраиваем встроенные или настраиваемые службы, которые нужны нашему приложению.
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            /*1*/
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));// задали директорию для хранения логов


        }


        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Метод используется именно для этого: настраивает наши службы
        /// Сервис - это многоразовая часть кода, которая добавляет некоторые функции нашему приложению.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllersWithViews(options => options.Filters.Add(new SimpleResourceFilter()));// применение созданного мной фильтра глобально.Юудет применятся ко всем контроллерам и методам в приложении
            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);// указываем карту для мапера
            services.ConfigureCors();
            services.ConfigureIISIntegration();//Традиционно веб-сервер IIS (Internet Information Services) применялся для развертывания веб-приложений
            /*2*/
            services.ConfigureLoggerService();//бобавляем метод из класса расширения чтобы не лепить сюда
            services.AddControllers();//(). Этот метод регистрирует контроллеры
            services.ConfigureSqlContext(Configuration);// расширенным метод для обращения к строке подключения
            services.ConfigureRepositoryManager();
            #region Описание методов
            /*
            }).AddXmlDataContractSerializerFormatters();// переопределяем сервер, чтобы можно было получить ответ в формате XML или другом

            services.AddControllers().AddNewtonsoftJson(options =>
                            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);*//*
            //тут мы прекращаем зацикливание. Когда ентити начинает сериализовать аккаунт он видит оунера, переходит 
            //в оунера и начинает сериализовать оунера, доходит до аккаунта и начинает серилизовать аккаун, где опять
            //натыкается на унера. Тут мы добавляем сервис, который это прекращает. Он добавляется через нунгепакет
            //Microsoft.AspNetCore.Mvc.NewtonsoftJson и внедряется через сервисы.
            //второй способ это в классе Owner над свойством   public ICollection<Account>  Accounts { get; set; }
            //добавить атрибут [Jsonignore] и все решится.
            //третий спосо через DTOUser в котором мы не будет ссылки на аккаунт.*/
            /*  services.AddControllers(config =>
              {
                  config.RespectBrowserAcceptHeader = true;
  */

            #endregion
            services.AddControllers(x => x.ReturnHttpNotAcceptable = true)//Мы добавили ReturnHttpNotAcceptable = true параметр, который сообщает серверу, что если клиент пытается согласовать тип носителя, который сервер не поддерживает, он должен вернуть 406 неприемлемо код состояния
                    .AddNewtonsoftJson()// для поддержки преобразования тела запроса в PatchDocument
                    .AddXmlDataContractSerializerFormatters()// переопределяем сервер, чтобы можно было получить ответ в формате XML или другом
                    .AddCustomCSVFormatter();// CsvOutputFormatter. Сервис для форматирования в csv формат. Форматирования расписано в классе CsvOutputFormatter
            //*!!!*/services.AddScoped<ValidationFilterAttribute>();// можно и нужно вынести в метод расшиерния
            services.AddScoped<CustomExceptionFilterAttribute>();//регистрируем фильтры
            services.AddScoped<ValidationFilterAttribute>();//регистрируем фильтры
            services.AddScoped<UserExistsAttribute>();
          //  services.AddScoped<LoggerResourceFilter>();//для того, чтобы можно было применять фильтры через внедрения зависимостей




            services.AddCors();//это механизм, который дает пользователю права доступа к ресурсам с сервера в другом домене
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// в Configure, мы собираемся добавить различные компоненты промежуточного программного обеспечения в конвейер запросов приложения.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(logger);//метод для обработки исключений
            app.UseHttpsRedirection();
            app.UseStaticFiles();//позволяет использовать статические файлы для запроса. Если мы не укажем путь к каталогу статических файлов, он будет использоватьwwwroot папка в нашем проекте по умолчанию.
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });//перенаправит заголовки прокси в текущий запрос. Это поможет нам при развертывании приложения.

            app.UseRouting();// добавляет функции маршрктизации
            app.UseAuthorization();//добавляет функции авторизации

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
