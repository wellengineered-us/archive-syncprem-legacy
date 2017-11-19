/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Utilities;

namespace TextMetal.Middleware.Solder.Executive
{
	public abstract class ConsoleApplicationFascade : ExecutableApplicationFascade
	{
		#region Constructors/Destructors

		protected ConsoleApplicationFascade(IDataTypeFascade dataTypeFascade, IAppConfigFascade appConfigFascade, IReflectionFascade reflectionFascade, IAssemblyInformationFascade assemblyInformationFascade)
			: base(dataTypeFascade, appConfigFascade, reflectionFascade, assemblyInformationFascade)
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// The entry point method for this application.
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public static int Run<TConsoleApp>(string[] args)
			where TConsoleApp : ConsoleApplicationFascade
		{
			using (TConsoleApp program = (TConsoleApp)AssemblyDomain.Default.DependencyManager.ResolveDependency<ConsoleApplicationFascade>(string.Empty, true))
				return program.EntryPoint(args);
		}

		protected sealed override void DisplayArgumentErrorMessage(IEnumerable<Message> argumentMessages)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			if ((object)argumentMessages != null)
			{
				Console.WriteLine();
				foreach (Message argumentMessage in argumentMessages)
					Console.WriteLine(argumentMessage.Description);
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected sealed override void DisplayArgumentMapMessage(IDictionary<string, ArgumentSpec> argumentMap)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Magenta;

			IEnumerable<string> requiredArgumentTokens = argumentMap.Select(m => (!m.Value.Required ? "[" : string.Empty) + string.Format("-{0}:value{1}", m.Key, !m.Value.Bounded ? "(s)" : string.Empty) + (!m.Value.Required ? "]" : string.Empty));

			if ((object)requiredArgumentTokens != null)
			{
				Console.WriteLine();
				// HACK
				Console.WriteLine(string.Format("USAGE: {0} ", this.AssemblyInformationFascade.ModuleName) + string.Join((string)" ", (IEnumerable<string>)requiredArgumentTokens));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected sealed override void DisplayBannerMessage()
		{
			Console.WriteLine(string.Format("{0} v{1} ({2}; {3})", this.AssemblyInformationFascade.ModuleName,
				this.AssemblyInformationFascade.NativeFileVersion, this.AssemblyInformationFascade.AssemblyVersion, this.AssemblyInformationFascade.InformationalVersion));
		}

		protected sealed override void DisplayFailureMessage(Exception exception)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine();
			Console.WriteLine((object)exception != null ? this.ReflectionFascade.GetErrors(exception, 0) : "<unknown>");
			Console.ForegroundColor = oldConsoleColor;

			Console.WriteLine();
			Console.WriteLine("The operation failed to complete.");
		}

		protected sealed override void DisplayRawArgumentsMessage(string[] args, IEnumerable<string> arguments)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Blue;

			if ((object)arguments != null)
			{
				Console.WriteLine();
				Console.WriteLine("RAW CMDLN: {0}", string.Join(" ", args));
				Console.WriteLine();
				Console.WriteLine("RAW ARGS:");

				int index = 0;
				foreach (string argument in arguments)
					Console.WriteLine("[{0}] => {1}", index++, argument);
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected sealed override void DisplaySuccessMessage(TimeSpan duration)
		{
			Console.WriteLine();
			Console.WriteLine("Operation completed successfully; duration: '{0}'.", duration);
		}

		#endregion
	}
}