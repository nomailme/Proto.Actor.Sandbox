using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;
using ServiceCollectionExtensions = Proto.ServiceCollectionExtensions;

namespace PersistentMailbox
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            var builder = new ContainerBuilder();
            services.AddMvc();

            ServiceCollectionExtensions.AddProtoActor(services,
                props => { props.RegisterProps<DepartmentManagerActor>(x => x); });


            builder.Populate(services);
            builder.RegisterType<ActorManager>();
            builder.Register(x => Log.Logger).As<ILogger>();

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddSerilog();
            Proto.Log.SetLoggerFactory(loggerFactory);
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseMvc();
//            app.Run(async context =>
//            {
//                await context.Response.WriteAsync("Hello World!");
//                var manager = ApplicationContainer.Resolve<ActorManager>().Get<DepartmentManagerActor>();
//                manager.Tell("string");
//            });

        }
    }
}