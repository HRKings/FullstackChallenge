// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Challenge.Auth
{
	public class Startup
	{
		public Startup(IWebHostEnvironment environment)
		{
			Environment = environment;
		}

		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader());
			});

			services.AddIdentityServer()
				.AddDeveloperSigningCredential()
				.AddInMemoryApiScopes(Config.GetApiScopes())
				.AddInMemoryIdentityResources(Config.GetIdentityResources())
				.AddInMemoryClients(Config.GetClients())
				.AddTestUsers(Config.GetUsers());

			services.AddSingleton<ICorsPolicyService>(container =>
			{
				var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
				return new DefaultCorsPolicyService(logger)
				{
					AllowAll = true
				};
			});
		}

		public void Configure(IApplicationBuilder app)
		{
			if (Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

			app.UseCookiePolicy();
			app.UseIdentityServer();

			app.UseStaticFiles();

			app.UseRouting();
			app.UseCors("CorsPolicy");

			app.UseAuthorization();
			app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
		}
	}
}