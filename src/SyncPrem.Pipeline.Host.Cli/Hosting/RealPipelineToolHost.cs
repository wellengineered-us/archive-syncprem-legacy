/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Host.Cli.Hosting
{
	public sealed class RealPipelineToolHost : PipelineComponent, IRealPipelineToolHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public RealPipelineToolHost()
		{
		}

		#endregion

		#region Methods/Operators

		private static TConfiguration FromJsonFile<TConfiguration>(string jsonFilePath)
			where TConfiguration : class, IConfigurationObject, new()
		{
			TConfiguration configuration;

			configuration = JsonSerializationStrategy.Instance.GetObjectFromFile<TConfiguration>(jsonFilePath);

			return configuration;
		}

		private static IEnumerable<IRecord> RecordsFromJsonFile(string jsonFilePath)
		{
			IEnumerable<IRecord> records;

			records = JsonSerializationStrategy.Instance.GetObjectFromFile<Record[]>(jsonFilePath);

			return records;
		}

		private static void RecordsToJsonFile(IEnumerable<IRecord> records, string jsonFilePath)
		{
			JsonSerializationStrategy.Instance.SetObjectToFile<IEnumerable<IRecord>>(jsonFilePath, records);
		}

		private static void ToJsonFile<TConfiguration>(TConfiguration configuration, string jsonFilePath)
			where TConfiguration : class, IConfigurationObject, new()
		{
			JsonSerializationStrategy.Instance.SetObjectToFile<TConfiguration>(jsonFilePath, configuration);
		}

		public void Host(RealPipelineConfiguration realPipelineConfiguration)
		{
			IRealPipeline realPipeline;
			Type realPipelineType;

			IEnumerable<Message> messages;

			if ((object)realPipelineConfiguration == null)
				throw new ArgumentNullException(nameof(realPipelineConfiguration));

			messages = realPipelineConfiguration.Validate();

			if (messages.Any())
				throw new InvalidOperationException(string.Format("RealPipelineConfiguration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));

			realPipelineType = realPipelineConfiguration.GetPipelineType();

			if ((object)realPipelineType == null)
				throw new InvalidOperationException(nameof(realPipelineType));

			realPipeline = (IRealPipeline)Activator.CreateInstance(realPipelineType);

			if ((object)realPipeline == null)
				throw new InvalidOperationException(nameof(realPipeline));

			using (realPipeline)
			{
				realPipeline.RealPipelineConfiguration = realPipelineConfiguration;
				realPipeline.Create();

				using (IPipelineContext pipelineContext = realPipeline.CreateContext())
				{
					pipelineContext.Create();

					realPipeline.Execute(pipelineContext);
				}
			}
		}

		public void Host(string sourceFilePath)
		{
			RealPipelineConfiguration realPipelineConfiguration;

			sourceFilePath = Path.GetFullPath(sourceFilePath);
			realPipelineConfiguration = FromJsonFile<RealPipelineConfiguration>(sourceFilePath);

			this.Host(realPipelineConfiguration);
		}

		#endregion
	}
}