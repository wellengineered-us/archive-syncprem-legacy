/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Oxymoron;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;

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
			realPipelineConfiguration = OxymoronEngine.FromJsonFile<RealPipelineConfiguration>(sourceFilePath);

			this.Host(realPipelineConfiguration);
		}

		#endregion
	}
}