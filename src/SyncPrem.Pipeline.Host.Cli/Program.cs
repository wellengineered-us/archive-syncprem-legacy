/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Diagnostics;

using SyncPrem.Pipeline.Host.Cli.Hosting;

using TextMetal.Middleware.Solder.Executive;

namespace SyncPrem.Pipeline.Host.Cli
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
		public static int Main(string[] args)
		{
			/*Console.CancelKeyPress += (s, e) =>
									{
										Debugger.Launch();
										e.Cancel = true;
									};*/

			return ConsoleApplicationFascade.Run<PipelineHostConsoleApplication>(args);
		}

		#endregion
	}
}