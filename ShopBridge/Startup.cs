using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Shop_Bridge.Common;
using Shop_Bridge.Common.DBConnection;
using Shop_Bridge.Common.Middleware;
using Shop_Bridge.Filters;
using LinqToDB.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shop_Bridge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ShopBridgePolicy",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpClient();
            LoadAssemblies();
            WireResolutions(services, Configuration);
            services.AddSwaggerDocument(document =>
            {
                document.Title = "ShopBridge";
                document.Version = "V1";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseOpenApi();
            app.UseReDoc(settings =>
            {
                settings.Path = string.Empty;
            });
            ConfigManager.Configuration = Configuration;
            DataConnection.DefaultSettings = new DBSettings();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("ShopBridgePolicy");
            app.UseMiddleware<LoggerMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            new AuthorizationFilter().Init(serviceProvider);
        }

        public void LoadAssemblies()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                FileInfo file = new FileInfo(dll);
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.Split(",")[0] == Path.GetFileNameWithoutExtension(dll)))
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
            }
        }

        readonly Action<IServiceCollection, IConfiguration> WireResolutions = (services, Configuration) =>
        {
            List<ResolutionElement> resolutionElements = Configuration.GetSection("ContainerResolverRepository")
                                                                      .GetChildren().ToList().Select(x => new ResolutionElement(
                                                                       x.GetValue<string>("contract"),
                                                                       x.GetValue<string>("implementation"))).ToList();

            resolutionElements.AddRange(Configuration.GetSection("ContainerResolverProvider")
                                                     .GetChildren().ToList().Select(x => new ResolutionElement(
                                                     x.GetValue<string>("contract"),
                                                     x.GetValue<string>("implementation"))).ToList());

            resolutionElements.ForEach(x =>
            {
                Type contract = Type.GetType(x.Contract);
                Type concreteImplementation = Type.GetType(x.Implementation);
                services.AddSingleton(contract, concreteImplementation);
            });
        };
    }
}
