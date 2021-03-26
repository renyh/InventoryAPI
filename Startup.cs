using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // ����ʱ��ʼ����־����дһ����־
            string path = AppContext.BaseDirectory;
            string logdir = path + @"log\";
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(logdir, "log_.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("Inventory API �����ˡ�");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Inventory API V1",
                    Description = "dp2 ͼ��ݼ���ϵͳ�̵� API",
                    Version = "v1",
                    License = new OpenApiLicense
                    {
                        Name = "Apache-2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                    },
                    Contact = new OpenApiContact
                    {
                        Name = "renyh",
                        Email = "renyh@dp2003.com",
                        Url = new Uri("https://github.com/renyh/InventoryAPI")
                    }
                });

            });


            // �Ժ��������µĽӿڰ汾��ʹ�ô����
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v2", new OpenApiInfo { Title = "", Version = "" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Inventory API V1");

                    // �Ժ��������µĽӿڰ汾��ʹ�ô����
                    //c.SwaggerEndpoint($"/swagger/v2/swagger.json", "Inventory API V1");
                });
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
