/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations.FlatText;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.Textual.Delimited;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.FlatText
{
	public class DelimitedTextDestinationConnector : DestinationConnector<DelimitedTextConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public DelimitedTextDestinationConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private DelimitedTextualWriter delimitedTextualWriter;

		#endregion

		#region Properties/Indexers/Events

		private DelimitedTextualWriter DelimitedTextualWriter
		{
			get
			{
				return this.delimitedTextualWriter;
			}
			set
			{
				this.delimitedTextualWriter = value;
			}
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

			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			var payloads = channel.Records.Select(p => p.Payload);

			this.DelimitedTextualWriter.WriteRecords(payloads);
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

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if ((object)this.DelimitedTextualWriter != null)
			{
				this.DelimitedTextualWriter.Flush();
				this.DelimitedTextualWriter.Dispose();
			}

			this.DelimitedTextualWriter = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			IDelimitedTextualSpec spec;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			spec = DelimitedTextSpecConfiguration.ToSpec(fsConfig.DelimitedTextSpecConfiguration);

			if ((object)spec == null)
				throw new SyncPremException(nameof(spec));

			if (!spec.DelimitedTextHeaderSpecs.Any())
				throw new InvalidOperationException(string.Format("Configuration missing: [Source and/or Destination]...{0}.{1}", nameof(DelimitedTextualSpec), nameof(DelimitedTextualSpec.DelimitedTextHeaderSpecs)));

			this.DelimitedTextualWriter = new DelimitedTextualWriter(new StreamWriter(File.Open(fsConfig.DelimitedTextFilePath, FileMode.Create, FileAccess.Write, FileShare.None)), spec);
		}

		#endregion
	}
}