using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SyncPrem.ReferenceTarget.Web
{
	public class Startup
	{
		#region Constructors/Destructors

		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		#endregion

		#region Properties/Indexers/Events

		public IConfiguration Configuration
		{
			get;
		}

		#endregion

		#region Methods/Operators

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc(routes =>
						{
							routes.MapRoute(
								name: "default",
								template: "",
								defaults: new
										{
											controller = "Test",
											action = "Index"
										});
						});
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
		}

		#endregion
	}
}