﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OSDevGrp.OSIntranet.WebApi.PostBuild;

namespace OSDevGrp.OSIntranet.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebApplication(args).Run();
        }

		private static WebApplication CreateWebApplication(string[] args)
        {
            WebApplicationBuilder applicationBuilder = WebApplication.CreateBuilder(args);
            applicationBuilder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

			Startup startup = new Startup(applicationBuilder.Configuration);
			startup.ConfigureServices(applicationBuilder.Services);

			WebApplication application = applicationBuilder.Build();
			startup.Configure(application, application.Environment);

            PostBuildExecutor.Execute(args, Startup.RunningInDocker, Startup.WebApiVersion, $"{typeof(Program).Namespace}.ClientApi", "WebApiClient", application.Services);

			return application;
		}
	}
}