/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.Textual;

namespace SyncPrem.Pipeline.Core.Sync.Connectors
{
	public abstract class TextualDestinationConnector<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec, TTextualConnectorSpecificConfiguration, TTextualWriter> : SyncDestinationConnector<TTextualConnectorSpecificConfiguration>
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
		where TTextualConnectorSpecificConfiguration : TextualConnectorSpecificConfiguration<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec>, new()
		where TTextualWriter : TextualWriter<TTextualFieldSpec, TTextualSpec>
	{
		#region Constructors/Destructors

		protected TextualDestinationConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private TTextualWriter textualWriter;

		#endregion

		#region Properties/Indexers/Events

		protected TTextualWriter TextualWriter
		{
			get
			{
				return this.textualWriter;
			}
			private set
			{
				this.textualWriter = value;
			}
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

			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			var payloads = channel.Records.Select(p => p.Payload);

			this.TextualWriter.WriteRecords(payloads);
		}

		protected override void Create(bool creating)
		{
			// do nothing
			base.Create(creating);
		}

		protected abstract TTextualWriter CreateTextualWriter(StreamWriter streamWriter, TTextualSpec textualSpec);

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if ((object)this.TextualWriter != null)
				{
					this.TextualWriter.Dispose();
					this.TextualWriter = null;
				}
			}

			base.Dispose(disposing);
		}

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)this.TextualWriter != null)
			{
				this.TextualWriter.Flush();
				this.TextualWriter.Dispose();
				this.TextualWriter = null;
			}
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			TTextualSpec spec;
			string filePath;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			spec = fsConfig.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new SyncPremException(nameof(spec));

			//if (!spec.TextualHeaderSpecs.Any())
			//throw new InvalidOperationException(string.Format("Configuration missing: [Source and/or Destination]...{0}.{1}", nameof(DelimitedTextualSpec), nameof(DelimitedTextualSpec.TextualHeaderSpecs)));

			if (Directory.Exists(fsConfig.TextualFilePath))
				filePath = Path.Combine(fsConfig.TextualFilePath, Path.GetRandomFileName());
			else
				filePath = fsConfig.TextualFilePath;

			this.TextualWriter = this.CreateTextualWriter(new StreamWriter(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None)), spec);
		}

		#endregion
	}
}