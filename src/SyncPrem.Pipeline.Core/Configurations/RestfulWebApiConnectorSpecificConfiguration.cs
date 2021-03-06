/*
	Copyright ?2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.StreamingIO.ProxyWrappers.Strategies;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public class RestfulWebApiConnectorSpecificConfiguration : StageSpecificConfiguration
	{
		#region Constructors/Destructors

		public RestfulWebApiConnectorSpecificConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string compressionStrategyAqtn;
		private string serializationStrategyAqtn;
		private string webEndpointUri;

		#endregion

		#region Properties/Indexers/Events

		public string CompressionStrategyAqtn
		{
			get
			{
				return this.compressionStrategyAqtn;
			}
			set
			{
				this.compressionStrategyAqtn = value;
			}
		}

		public string SerializationStrategyAqtn
		{
			get
			{
				return this.serializationStrategyAqtn;
			}
			set
			{
				this.serializationStrategyAqtn = value;
			}
		}

		public string WebEndpointUri
		{
			get
			{
				return this.webEndpointUri;
			}
			set
			{
				this.webEndpointUri = value;
			}
		}

		#endregion

		#region Methods/Operators

		public Type GetCompressionStrategyType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.CompressionStrategyAqtn, messages);
		}

		public Type GetSerializationStrategyType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.SerializationStrategyAqtn, messages);
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			string adapterContext;

			Type compressionStrategyType;
			Type serializationStrategyType;

			ICompressionStrategy compressionStrategy;
			ISerializationStrategy serializationStrategy;

			adapterContext = context as string;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.WebEndpointUri))
				messages.Add(NewError(string.Format("{0} adapter web endpoint URI is required.", adapterContext)));

			if (!Uri.TryCreate(this.WebEndpointUri, UriKind.Absolute, out Uri uri))
				messages.Add(NewError(string.Format("{0} adapter web endpoint URI is invalid.", adapterContext)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.CompressionStrategyAqtn))
				messages.Add(NewError(string.Format("{0} adapter compression strategy is required.", adapterContext)));
			else
			{
				compressionStrategyType = this.GetCompressionStrategyType(messages);

				if ((object)compressionStrategyType == null)
					messages.Add(NewError(string.Format("{0} adapter compression strategy failed to load type from AQTN.", adapterContext)));
				else if (!typeof(ICompressionStrategy).IsAssignableFrom(compressionStrategyType))
					messages.Add(NewError(string.Format("{0} adapter compression strategy loaded an unrecognized type via AQTN.", adapterContext)));
				else
				{
					// new-ing up via default public contructor should be low friction
					compressionStrategy = (ICompressionStrategy)Activator.CreateInstance(compressionStrategyType);

					if ((object)compressionStrategy == null)
						messages.Add(NewError(string.Format("{0} adapter compression strategy failed to instatiate type from AQTN.", adapterContext)));
				}
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.SerializationStrategyAqtn))
				messages.Add(NewError(string.Format("{0} adapter serialization strategy is required.", adapterContext)));
			else
			{
				serializationStrategyType = this.GetSerializationStrategyType(messages);

				if ((object)serializationStrategyType == null)
					messages.Add(NewError(string.Format("{0} adapter serialization strategy failed to load type from AQTN.", adapterContext)));
				else if (!typeof(ISerializationStrategy).IsAssignableFrom(serializationStrategyType))
					messages.Add(NewError(string.Format("{0} adapter serialization strategy loaded an unrecognized type via AQTN.", adapterContext)));
				else
				{
					// new-ing up via default public contructor should be low friction
					serializationStrategy = (ISerializationStrategy)Activator.CreateInstance(serializationStrategyType);

					if ((object)serializationStrategy == null)
						messages.Add(NewError(string.Format("{0} adapter serialization strategy failed to instatiate type from AQTN.", adapterContext)));
				}
			}

			return messages;
		}

		#endregion
	}
}