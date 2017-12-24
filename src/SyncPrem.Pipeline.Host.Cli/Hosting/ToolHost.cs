/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Host.Cli.Hosting
{
	public sealed class ToolHost : Component, IToolHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public ToolHost()
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

		public void Host(PipelineConfiguration pipelineConfiguration)
		{
			IPipeline pipeline;
			Type realPipelineType;

			IEnumerable<Message> messages;

			if ((object)pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			messages = pipelineConfiguration.Validate();

			if (messages.Any())
				throw new InvalidOperationException(string.Format("PipelineConfiguration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));

			realPipelineType = pipelineConfiguration.GetPipelineType();

			if ((object)realPipelineType == null)
				throw new InvalidOperationException(nameof(realPipelineType));

			pipeline = (IPipeline)Activator.CreateInstance(realPipelineType);

			if ((object)pipeline == null)
				throw new InvalidOperationException(nameof(pipeline));

			using (pipeline)
			{
				pipeline.PipelineConfiguration = pipelineConfiguration;
				pipeline.Create();

				using (IContext context = pipeline.CreateContext())
				{
					context.Create();

					pipeline.Execute(context);
				}
			}
		}

		public void Host(string sourceFilePath)
		{
			PipelineConfiguration pipelineConfiguration;

			sourceFilePath = Path.GetFullPath(sourceFilePath);
			pipelineConfiguration = FromJsonFile<PipelineConfiguration>(sourceFilePath);

			this.Host(pipelineConfiguration);
		}

		#endregion
	}
}