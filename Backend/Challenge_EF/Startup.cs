using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Challenge_EF.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Challenge_EF
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
			services.AddCors(options => options.AddPolicy("Default", builder =>
					builder.WithOrigins(Environment.GetEnvironmentVariable("FRONTEND_PATH")).AllowCredentials().AllowAnyHeader().AllowAnyMethod()
					));
			
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo {Title = "Challenge_EF", Version = "v1"});
				
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = @"JWT Authorization header using the Bearer scheme.<br/>
                      Enter 'Bearer' [space] and then your token in the text input below.<br/>
                      Example: 'Bearer 12345abcdef'",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header,

						},
						new List<string>()
					}
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			services.AddDbContext<ChallengeDbContext>(
				options => options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE")));
			
			services.AddAuthentication("Bearer")
				.AddJwtBearer("Bearer", options =>
				{
					options.Authority = Environment.GetEnvironmentVariable("IDENTITY_SERVER");

					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = false,
					};
				});

			IdentityModelEventSource.ShowPII = true;
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge_EF v1"));
			}
			
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("Default");
			
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}