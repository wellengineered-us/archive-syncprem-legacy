/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Host.Cli.Async.Hosting;

using TextMetal.Middleware.Solder.Executive;

namespace SyncPrem.Pipeline.Host.Cli.Async
{
	/// <summary>
	/// Entry point static class for the application.
	/// </summary>
	internal static class Program
	{
		#region Methods/Operators

		/// <summary>
		/// The entry point method for this application.
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		[STAThread]
		public static async Task<int> Main(string[] args)
		{
			return await AsyncExecutableApplicationFascade.Run<PipelineHostConsoleApplication>(args);
		}

		#endregion
	}
}