/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers.Internal;
using SyncPrem.StreamingIO.ProxyWrappers.Strategies;

using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Core.Sync.Connectors
{
	public class RestfulWebApiDestinationConnector : SyncDestinationConnector<RestfulWebApiConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public RestfulWebApiDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeInternal(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel)
		{
			IEnumerable<ISyncRecord> records;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			using (HttpClient httpClient = new HttpClient())
			{
				using (HttpContent httpContent = new PushStreamContent((s) => this.SerializeRecordsToStream(s, records)))
				{
					using (HttpResponseMessage result = httpClient.PostAsync(fsConfig.WebEndpointUri, httpContent).GetAwaiter().GetResult())
					{
						result.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
						result.EnsureSuccessStatusCode();
					}
				}
			}
		}

		protected override void Create(bool creating)
		{
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
			base.Dispose(disposing);
		}

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;
		}

		/// <summary>
		/// https://blogs.msdn.microsoft.com/henrikn/2012/02/16/push-and-pull-streams-using-httpclient/
		/// </summary>
		/// <param name="stream"> </param>
		/// <param name="records"> </param>
		/// <returns> </returns>
		private void SerializeRecordsToStream(Stream stream, IEnumerable<IRecord> records)
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

			protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
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