using System;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SyncPrem.Controller.Web
{
	public class Program
	{
		#region Methods/Operators

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();

		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		#endregion
	}
}