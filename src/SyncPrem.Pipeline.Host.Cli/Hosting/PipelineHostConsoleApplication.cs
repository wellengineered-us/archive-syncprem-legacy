/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TextMetal.Middleware.Solder.Executive;
using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Utilities;

namespace SyncPrem.Pipeline.Host.Cli.Hosting
{
	/// <summary>
	/// Entry point class for the application.
	/// </summary>
	public class PipelineHostConsoleApplication : ConsoleApplicationFascade
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public PipelineHostConsoleApplication([DependencyInjection] IDataTypeFascade dataTypeFascade, [DependencyInjection] IAppConfigFascade appConfigFascade, [DependencyInjection] IReflectionFascade reflectionFascade, [DependencyInjection] IAssemblyInformationFascade assemblyInformationFascade)
			: base(dataTypeFascade, appConfigFascade, reflectionFascade, assemblyInformationFascade)
		{
		}

		#endregion

		#region Fields/Constants

		private const string CMDLN_TOKEN_PROPERTY = "property";
		private const string CMDLN_TOKEN_SOURCEFILE = "sourcefile";

		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		#endregion

		#region Properties/Indexers/Events

		private CancellationTokenSource CancellationTokenSource
		{
			get
			{
				return this.cancellationTokenSource;
			}
		}

		#endregion

		#region Methods/Operators

		[DependencyMagicMethod]
		public static void OnDependencyMagic(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			dependencyManager.AddResolution<ConsoleApplicationFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<ConsoleApplicationFascade>(TransientActivatorAutoWiringDependencyResolution<PipelineHostConsoleApplication>.From(dependencyManager)));
			dependencyManager.AddResolution<IToolHost>(string.Empty, false, new SingletonWrapperDependencyResolution<IToolHost>(TransientActivatorAutoWiringDependencyResolution<ToolHost>.From(dependencyManager)));
			dependencyManager.AddResolution<IAdoNetBufferingFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<IAdoNetBufferingFascade>(TransientActivatorAutoWiringDependencyResolution<AdoNetBufferingFascade>.From(dependencyManager)));
		}

		protected override IDictionary<string, ArgumentSpec> GetArgumentMap()
		{
			IDictionary<string, ArgumentSpec> argumentMap;

			argumentMap = new Dictionary<string, ArgumentSpec>();
			argumentMap.Add(CMDLN_TOKEN_SOURCEFILE, new ArgumentSpec<string>(true, true));
			argumentMap.Add(CMDLN_TOKEN_PROPERTY, new ArgumentSpec<string>(false, false));

			return argumentMap;
		}

		protected override bool OnCancelKeySignal(ConsoleSpecialKey consoleSpecialKey)
		{
			if (consoleSpecialKey == ConsoleSpecialKey.ControlC)
			{
				this.CancellationTokenSource.Cancel();
				return true;
			}

			return base.OnCancelKeySignal(consoleSpecialKey);
		}

		protected override async Task<int> OnStartup(string[] args, IDictionary<string, IList<object>> arguments)
		{
			Dictionary<string, object> argz;
			string sourceFilePath;
			IDictionary<string, IList<string>> properties;
			IList<object> argumentValues;
			IList<string> propertyValues;
			bool hasProperties;

			if ((object)args == null)
				throw new ArgumentNullException(nameof(args));

			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			// required
			properties = new Dictionary<string, IList<string>>();

			sourceFilePath = (string)arguments[CMDLN_TOKEN_SOURCEFILE].Single();

			hasProperties = arguments.TryGetValue(CMDLN_TOKEN_PROPERTY, out argumentValues);

			argz = new Dictionary<string, object>();
			argz.Add(CMDLN_TOKEN_SOURCEFILE, sourceFilePath);
			argz.Add(CMDLN_TOKEN_PROPERTY, hasProperties ? (object)argumentValues : null);

			if (hasProperties)
			{
				if ((object)argumentValues != null)
				{
					foreach (string argumentValue in argumentValues)
					{
						string key, value;

						if (!this.TryParseCommandLineArgumentProperty(argumentValue, out key, out value))
							continue;

						if (!properties.ContainsKey(key))
							properties.Add(key, propertyValues = new List<string>());
						else
							propertyValues = properties[key];

						// duplicate values are ignored
						if (propertyValues.Contains(value))
							continue;

						propertyValues.Add(value);
					}
				}
			}

			using (IToolHost toolHost = AssemblyDomain.Default.DependencyManager.ResolveDependency<IToolHost>(string.Empty, true))
			{
				await toolHost.RunAsync(sourceFilePath, this.CancellationTokenSource.Token);
			}

			return 0;
		}

		#endregion
	}
}