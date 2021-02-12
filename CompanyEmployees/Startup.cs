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
    /// � ������ Startup �� ����������� ���������� ��� ������������� ������, ������� ����� ������ ����������.
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            /*1*/
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));// ������ ���������� ��� �������� �����


        }


        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// ����� ������������ ������ ��� �����: ����������� ���� ������
        /// ������ - ��� ������������ ����� ����, ������� ��������� ��������� ������� ������ ����������.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllersWithViews(options => options.Filters.Add(new SimpleResourceFilter()));// ���������� ���������� ���� ������� ���������.����� ���������� �� ���� ������������ � ������� � ����������
            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);// ��������� ����� ��� ������
            services.ConfigureCors();
            services.ConfigureIISIntegration();//����������� ���-������ IIS (Internet Information Services) ���������� ��� ������������� ���-����������
            /*2*/
            services.ConfigureLoggerService();//��������� ����� �� ������ ���������� ����� �� ������ ����
            services.AddControllers();//(). ���� ����� ������������ �����������
            services.ConfigureSqlContext(Configuration);// ����������� ����� ��� ��������� � ������ �����������
            services.ConfigureRepositoryManager();
            #region �������� �������
            /*
            }).AddXmlDataContractSerializerFormatters();// �������������� ������, ����� ����� ���� �������� ����� � ������� XML ��� ������

            services.AddControllers().AddNewtonsoftJson(options =>
                            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);*//*
            //��� �� ���������� ������������. ����� ������ �������� ������������� ������� �� ����� ������, ��������� 
            //� ������ � �������� ������������� ������, ������� �� �������� � �������� ������������ ������, ��� �����
            //���������� �� �����. ��� �� ��������� ������, ������� ��� ����������. �� ����������� ����� ����������
            //Microsoft.AspNetCore.Mvc.NewtonsoftJson � ���������� ����� �������.
            //������ ������ ��� � ������ Owner ��� ���������   public ICollection<Account>  Accounts { get; set; }
            //�������� ������� [Jsonignore] � ��� �������.
            //������ ����� ����� DTOUser � ������� �� �� ����� ������ �� �������.*/
            /*  services.AddControllers(config =>
              {
                  config.RespectBrowserAcceptHeader = true;
  */

            #endregion
            services.AddControllers(x => x.ReturnHttpNotAcceptable = true)//�� �������� ReturnHttpNotAcceptable = true ��������, ������� �������� �������, ��� ���� ������ �������� ����������� ��� ��������, ������� ������ �� ������������, �� ������ ������� 406 ����������� ��� ���������
                    .AddNewtonsoftJson()// ��� ��������� �������������� ���� ������� � PatchDocument
                    .AddXmlDataContractSerializerFormatters()// �������������� ������, ����� ����� ���� �������� ����� � ������� XML ��� ������
                    .AddCustomCSVFormatter();// CsvOutputFormatter. ������ ��� �������������� � csv ������. �������������� ��������� � ������ CsvOutputFormatter
            //*!!!*/services.AddScoped<ValidationFilterAttribute>();// ����� � ����� ������� � ����� ����������
            services.AddScoped<CustomExceptionFilterAttribute>();//������������ �������
            services.AddScoped<ValidationFilterAttribute>();//������������ �������
            services.AddScoped<UserExistsAttribute>();
          //  services.AddScoped<LoggerResourceFilter>();//��� ����, ����� ����� ���� ��������� ������� ����� ��������� ������������




            services.AddCors();//��� ��������, ������� ���� ������������ ����� ������� � �������� � ������� � ������ ������
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// � Configure, �� ���������� �������� ��������� ���������� �������������� ������������ ����������� � �������� �������� ����������.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(logger);//����� ��� ��������� ����������
            app.UseHttpsRedirection();
            app.UseStaticFiles();//��������� ������������ ����������� ����� ��� �������. ���� �� �� ������ ���� � �������� ����������� ������, �� ����� ������������wwwroot ����� � ����� ������� �� ���������.
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });//������������ ��������� ������ � ������� ������. ��� ������� ��� ��� ������������� ����������.

            app.UseRouting();// ��������� ������� �������������
            app.UseAuthorization();//��������� ������� �����������

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
