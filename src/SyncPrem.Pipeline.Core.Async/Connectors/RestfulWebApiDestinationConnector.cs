/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers.Internal;
using SyncPrem.StreamingIO.ProxyWrappers.Strategies;

using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class RestfulWebApiDestinationConnector : AsyncDestinationConnector<RestfulWebApiConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public RestfulWebApiDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override async Task ConsumeAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IRecord> records;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			records = asyncChannel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			using (HttpClient httpClient = new HttpClient())
			{
				using (HttpContent httpContent = new PushStreamContent((s) => this.SerializeRecordsToStream(s, records)))
				{
					using (HttpResponseMessage result = await httpClient.PostAsync(fsConfig.WebEndpointUri, httpContent))
					{
						await result.Content.ReadAsStreamAsync();
						result.EnsureSuccessStatusCode();
					}
				}
			}
		}

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			// do nothing
			await base.CreateAsync(creating, cancellationToken);
		}

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			// do nothing
			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			return Task.CompletedTask;
		}

		protected override Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			return Task.CompletedTask;
		}

		/// <summary>
		/// https://blogs.msdn.microsoft.com/henrikn/2012/02/16/push-and-pull-streams-using-httpclient/
		/// </summary>
		/// <param name="stream"> </param>
		/// <param name="records"> </param>
		/// <returns> </returns>
		private void SerializeRecordsToStream(Stream stream, IAsyncEnumerable<IRecord> records)
		{
			Type serializationStrategyType;
			Type compressionStrategyType;

			ISerializationStrategy serializationStrategy;
			ICompressionStrategy compressionStrategy;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			serializationStrategyType = fsConfig.GetSerializationStrategyType();

			if ((object)serializationStrategyType == null)
				throw new InvalidOperationException(nameof(serializationStrategyType));

			serializationStrategy = (ISerializationStrategy)Activator.CreateInstance(serializationStrategyType);

			if ((object)serializationStrategy == null)
				throw new InvalidOperationException(nameof(serializationStrategy));

			compressionStrategyType = fsConfig.GetCompressionStrategyType();

			if ((object)compressionStrategyType == null)
				throw new InvalidOperationException(nameof(compressionStrategyType));

			compressionStrategy = (ICompressionStrategy)Activator.CreateInstance(compressionStrategyType);

			if ((object)compressionStrategy == null)
				throw new InvalidOperationException(nameof(compressionStrategy));

			stream = compressionStrategy.ApplyStreamWrap(stream);
			stream = new ProgressWrappedStream(stream); // uncompressed
			serializationStrategy.SetObjectToStream(stream, records);
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		private class PushStreamContent : HttpContent
		{
			#region Constructors/Destructors

			public PushStreamContent(Action<Stream> streamCallback)
			{
				if ((object)streamCallback == null)
					throw new ArgumentNullException(nameof(streamCallback));

				this.streamCallback = streamCallback;
			}

			#endregion

			#region Fields/Constants

			private readonly Action<Stream> streamCallback;

			#endregion

			#region Properties/Indexers/Events

			private Action<Stream> StreamCallback
			{
				get
				{
					return this.streamCallback;
				}
			}

			#endregion

			#region Methods/Operators

			protected override Task SerializeToStreamAsync(Stream stream, TransportContext asyncContext)
			{
				return Task.Factory.StartNew(
					(obj) =>
					{
						Stream target;
						target = obj as Stream;
						this.StreamCallback(target);
					},
					stream);
			}

			protected override bool TryComputeLength(out long length)
			{
				length = -1;
				return false;
			}

			#endregion
		}

		#endregion
	}
}