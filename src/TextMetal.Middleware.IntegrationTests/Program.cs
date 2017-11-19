/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using NUnit.Common;

using NUnitLite;

namespace TextMetal.Middleware.IntegrationTests
{
	/// <summary>
	/// Entry point class for the application.
	/// </summary>
	internal class Program
	{
		#region Constructors/Destructors

		public Program()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// The entry point method for this application.
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		[STAThread]
		public static int Main(string[] args)
		{
			return new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args, new ColorConsoleWriter(), Console.In);
		}

		#endregion
	}
}