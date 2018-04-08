/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Host.Cli.Hosting
{
	public static class ToolHostExtensions
	{
		#region Methods/Operators

		private static TConfiguration FromJsonFile<TConfiguration>(string jsonFilePath)
			where TConfiguration : class, IConfigurationObject, new()
		{
			TConfiguration configuration;
			ISerializationStrategy serializationStrategy;

			serializationStrategy = new JsonSerializationStrategy();
			configuration = serializationStrategy.GetObjectFromFile<TConfiguration>(jsonFilePath);

			return configuration;
		}

		public static async Task RunAsync(this IToolHost toolHost, string sourceFilePath, CancellationToken cancellationToken)
		{
			HostConfiguration hostConfiguration;

			if ((object)toolHost == null)
				throw new ArgumentNullException(nameof(toolHost));

			sourceFilePath = Path.GetFullPath(sourceFilePath);
			hostConfiguration = FromJsonFile<HostConfiguration>(sourceFilePath);

			toolHost.Configuration = hostConfiguration;
			toolHost.Create();
			await toolHost.RunAsync(cancellationToken);
		}

		#endregion
	}
}