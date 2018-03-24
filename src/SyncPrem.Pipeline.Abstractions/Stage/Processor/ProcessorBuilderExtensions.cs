/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Stage.Processor
{
	public static class ProcessorBuilderExtensions
	{
		#region Methods/Operators

		public static IProcessorBuilder UseMiddleware(this IProcessorBuilder processorBuilder, IProcessor processor, StageConfiguration stageConfiguration)
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
														IProcessor _processor = processor; // prevent closure bug
														IChannel newChannel;

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

		public static IProcessorBuilder UseMiddleware(this IProcessorBuilder processorBuilder, Type processorType, StageConfiguration stageConfiguration)
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
														IProcessor processor;
														IChannel newChannel;

														processor = (IProcessor)Activator.CreateInstance(processorType);

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