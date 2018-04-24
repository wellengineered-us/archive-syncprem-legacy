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
	public abstract class SyncExecutableApplicationFascade : ExecutableApplicationFascade, ISyncExecutableApplicationFascade
	{
		#region Constructors/Destructors

		protected SyncExecutableApplicationFascade(IDataTypeFascade dataTypeFascade, IAppConfigFascade appConfigFascade, IReflectionFascade reflectionFascade, IAssemblyInformationFascade assemblyInformationFascade)
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
			where TConsoleApp : SyncExecutableApplicationFascade
		{
			using (TConsoleApp program = (TConsoleApp)AssemblyDomain.Default.DependencyManager.ResolveDependency<SyncExecutableApplicationFascade>(string.Empty, true))
			{
				program.Create();
				return program.EntryPoint(args);
			}
		}

		/// <summary>
		/// The indirect entry point method for this application. Code is wrapped in this method to leverage the 'TryStartup'/'Startup' pattern. This method, if used, wraps the Startup() method in an exception handler. The handler will catch all exceptions and report a full detailed stack trace to the Console.Error stream; -1 is then returned as the exit code. Otherwise, if no exception is thrown, the exit code returned is that which is returned by Startup().
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public int EntryPoint(string[] args)
		{
			this.MaybeLaunchDebugger();

			if (this.HookUnhandledExceptions)
				return this.TryStartup(args);
			else
				return this.Startup(args);
		}

		protected abstract int OnStartup(string[] args, IDictionary<string, IList<object>> arguments);

		private int Startup(string[] args)
		{
			int returnCode;
			DateTime start, end;
			TimeSpan duration;
			IDictionary<string, ArgumentSpec> argumentMap;
			IList<Message> argumentValidationMessages;

			IList<string> argumentValues;
			IDictionary<string, IList<string>> arguments;

			IDictionary<string, IList<object>> finalArguments;
			IList<object> finalArgumentValues;
			object finalArgumentValue;

			this.DisplayBannerMessage();
			start = DateTime.UtcNow;

			arguments = this.ParseCommandLineArguments(args);
			argumentMap = this.GetArgumentMap();

			finalArguments = new Dictionary<string, IList<object>>();
			argumentValidationMessages = new List<Message>();

			if ((object)argumentMap != null)
			{
				foreach (string argumentToken in argumentMap.Keys)
				{
					bool argumentExists;
					int argumentValueCount = 0;
					ArgumentSpec argumentSpec;

					if (argumentExists = arguments.TryGetValue(argumentToken, out argumentValues))
						argumentValueCount = argumentValues.Count;

					if (!argumentMap.TryGetValue(argumentToken, out argumentSpec))
						continue;

					if (argumentSpec.Required && !argumentExists)
					{
						argumentValidationMessages.Add(new Message(string.Empty, string.Format("A required argument was not specified: '{0}'.", argumentToken), Severity.Error));
						continue;
					}

					if (argumentSpec.Bounded && argumentValueCount > 1)
					{
						argumentValidationMessages.Add(new Message(string.Empty, string.Format("A bounded argument was specified more than once: '{0}'.", argumentToken), Severity.Error));
						continue;
					}

					if ((object)argumentValues != null)
					{
						finalArgumentValues = new List<object>();

						if ((object)argumentSpec.Type != null)
						{
							foreach (string argumentValue in argumentValues)
							{
								if (!this.DataTypeFascade.TryParse(argumentSpec.Type, argumentValue, out finalArgumentValue))
									argumentValidationMessages.Add(new Message(string.Empty, string.Format("An argument '{0}' value '{1}' was specified that failed to parse to the target type '{2}'.", argumentToken, argumentValue, argumentSpec.Type.FullName), Severity.Error));
								else
									finalArgumentValues.Add(finalArgumentValue);
							}
						}
						else
						{
							foreach (string argumentValue in argumentValues)
								finalArgumentValues.Add(argumentValue);
						}

						finalArguments.Add(argumentToken, finalArgumentValues);
					}
				}
			}

			if (argumentValidationMessages.Any())
			{
				this.DisplayArgumentErrorMessage(argumentValidationMessages);
				this.DisplayArgumentMapMessage(argumentMap);
				//this.DisplayRawArgumentsMessage(args);
				returnCode = -1;
			}
			else
				returnCode = this.OnStartup(args, finalArguments);

			end = DateTime.UtcNow;
			duration = end - start;

			this.DisplaySuccessMessage(duration);

			return returnCode;
		}

		private int TryStartup(string[] args)
		{
			try
			{
				return this.Startup(args);
			}
			catch (Exception ex)
			{
				return this.ShowNestedExceptionsAndThrowBrickAtProcess(new Exception("Main", ex));
			}
		}

		#endregion
	}
}