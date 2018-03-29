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

using Newtonsoft.Json;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers.Internal;

namespace SyncPrem.Pipeline.Core.Connectors
{
	public class RestfulWebApiDestinationConnector : DestinationConnector<RestfulWebApiConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public RestfulWebApiDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeRecord(IContext context, RecordConfiguration configuration, IChannel channel)
		{
			IEnumerable<IRecord> records;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;
//"http://localhost:57087/api/values"
			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			using (HttpClient httpClient = new HttpClient())
			{
				using (HttpContent httpContent = new PushStreamContent((s) => this.SerializeRecordsToStream(new ProgressWrappedStream(s), records)))
				{
					using (Task<HttpResponseMessage> streamTask = httpClient.PostAsync(fsConfig.WebEndpointUri, httpContent))
					{
						streamTask.Wait();
						HttpResponseMessage result = streamTask.Result;
						Console.WriteLine(result.StatusCode);
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			RestfulWebApiConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration configuration)
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
			StreamWriter streamWriter;
			JsonTextWriter jsonTextWriter;
			JsonSerializer jsonSerializer;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			// TODO: verify if we should dispose here since we do not own the stream technically
			using (streamWriter = new StreamWriter(stream))
			{
				using (jsonTextWriter = new JsonTextWriter(streamWriter))
				{
					jsonSerializer = new JsonSerializer();
					jsonSerializer.Serialize(jsonTextWriter, records);
				}
			}
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