/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Messages
{
	public sealed class DefaultPipelineMetadata : PipelineComponent, IPipelineMetadata
	{
		#region Constructors/Destructors

		public DefaultPipelineMetadata(IEnumerable<IField> upstreamMetaData)
		{
			if ((object)upstreamMetaData == null)
				throw new ArgumentNullException(nameof(upstreamMetaData));

			this.upstreamMetaData = upstreamMetaData;
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<IField> upstreamMetaData;

		#endregion

		#region Properties/Indexers/Events

		public IEnumerable<IField> UpstreamFields
		{
			get
			{
				return this.upstreamMetaData;
			}
		}

		#endregion
	}
}