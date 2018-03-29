/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

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

		private string connectionString;

		private string webEndpointUri;

		#endregion

		#region Properties/Indexers/Events

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

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			string adapterContext;

			adapterContext = context as string;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.WebEndpointUri))
				messages.Add(NewError(string.Format("{0} adapter web endpoint URI is required.", adapterContext)));

			return messages;
		}

		#endregion
	}
}