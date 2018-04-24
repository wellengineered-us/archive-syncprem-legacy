/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor
{
	public static class SyncProcessorBuilderExtensions
	{
		#region Methods/Operators

		public static ISyncProcessorBuilder UseMiddleware(this ISyncProcessorBuilder processorBuilder, ISyncProcessor processor, StageConfiguration stageConfiguration)
		{
			if ((object)processorBuilder == null)
				throw new ArgumentNullException(nameof(processorBuilder));

			if ((object)processor == null)
				throw new ArgumentNullException(nameof(processor));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return processorBuilder.Use(next =>
										{
											return (context, configuration, channel) =>
													{
														ISyncProcessor _processor = processor; // prevent closure bug
														ISyncChannel newChannel;

														using (_processor)
														{
															_processor.Configuration = stageConfiguration;
															_processor.Create();

															_processor.PreExecute(context, configuration);
															newChannel = _processor.Process(context, configuration, channel, next);
															_processor.PostExecute(context, configuration);

															return newChannel;
														}
													};
										});
		}

		public static ISyncProcessorBuilder UseMiddleware(this ISyncProcessorBuilder processorBuilder, Type processorType, StageConfiguration stageConfiguration)
		{
			if ((object)processorBuilder == null)
				throw new ArgumentNullException(nameof(processorBuilder));

			if ((object)processorType == null)
				throw new ArgumentNullException(nameof(processorType));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return processorBuilder.Use(next =>
										{
											return (context, configuration, channel) =>
													{
														ISyncProcessor processor;
														ISyncChannel newChannel;

														processor = (ISyncProcessor)Activator.CreateInstance(processorType);

														if ((object)processor == null)
															throw new InvalidOperationException(nameof(processor));

														using (processor)
														{
															processor.Configuration = stageConfiguration;
															processor.Create();

															processor.PreExecute(context, configuration);
															newChannel = processor.Process(context, configuration, channel, next);
															processor.PostExecute(context, configuration);

															return newChannel;
														}
													};
										});
		}

		#endregion
	}
}