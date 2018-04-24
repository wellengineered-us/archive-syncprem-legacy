/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Processor
{
	public static class AsyncProcessorBuilderExtensions
	{
		#region Methods/Operators

		public static IAsyncProcessorBuilder UseMiddlewareAsync(this IAsyncProcessorBuilder asyncProcessorBuilder, IAsyncProcessor asyncProcessor, StageConfiguration stageConfiguration)
		{
			if ((object)asyncProcessorBuilder == null)
				throw new ArgumentNullException(nameof(asyncProcessorBuilder));

			if ((object)asyncProcessor == null)
				throw new ArgumentNullException(nameof(asyncProcessor));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return asyncProcessorBuilder.UseAsync(asyncNext =>
												{
													return async (asyncContext, configuration, asyncChannel, cancellationToken) =>
															{
																IAsyncProcessor _asyncProcessor = asyncProcessor; // prevent closure bug
																IAsyncChannel newAsyncChannel;

																using (AsyncDisposal.Await(_asyncProcessor, cancellationToken))
																{
																	_asyncProcessor.Configuration = stageConfiguration;
																	await _asyncProcessor.CreateAsync(cancellationToken);

																	await _asyncProcessor.PreExecuteAsync(asyncContext, configuration, cancellationToken);
																	newAsyncChannel = await _asyncProcessor.ProcessAsync(asyncContext, configuration, asyncChannel, asyncNext, cancellationToken);
																	await _asyncProcessor.PostExecuteAsync(asyncContext, configuration, cancellationToken);

																	return newAsyncChannel;
																}
															};
												});
		}

		public static IAsyncProcessorBuilder UseMiddlewareAsync(this IAsyncProcessorBuilder asyncProcessorBuilder, Type asyncProcessorType, StageConfiguration stageConfiguration)
		{
			if ((object)asyncProcessorBuilder == null)
				throw new ArgumentNullException(nameof(asyncProcessorBuilder));

			if ((object)asyncProcessorType == null)
				throw new ArgumentNullException(nameof(asyncProcessorType));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return asyncProcessorBuilder.UseAsync(asyncNext =>
												{
													return async (asyncContext, configuration, asyncChannel, cancellationToken) =>
															{
																IAsyncProcessor asyncProcessor;
																IAsyncChannel newAsyncChannel;

																asyncProcessor = (IAsyncProcessor)Activator.CreateInstance(asyncProcessorType);

																if ((object)asyncProcessor == null)
																	throw new InvalidOperationException(nameof(asyncProcessor));

																using (AsyncDisposal.Await(asyncProcessor, cancellationToken))
																{
																	asyncProcessor.Configuration = stageConfiguration;
																	await asyncProcessor.CreateAsync(cancellationToken);

																	await asyncProcessor.PreExecuteAsync(asyncContext, configuration, cancellationToken);
																	newAsyncChannel = await asyncProcessor.ProcessAsync(asyncContext, configuration, asyncChannel, asyncNext, cancellationToken);
																	await asyncProcessor.PostExecuteAsync(asyncContext, configuration, cancellationToken);

																	return newAsyncChannel;
																}
															};
												});
		}

		#endregion
	}
}