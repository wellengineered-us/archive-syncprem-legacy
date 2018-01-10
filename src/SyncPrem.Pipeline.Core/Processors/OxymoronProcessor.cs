/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;
using SyncPrem.StreamingIO.DataMasking;

namespace SyncPrem.Pipeline.Core.Processors
{
	public class OxymoronProcessor : Processor<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public OxymoronProcessor()
		{
		}

		#endregion

		#region Fields/Constants

		private IOxymoronEngine oxymoronEngine;

		#endregion

		#region Properties/Indexers/Events

		protected IOxymoronEngine OxymoronEngine
		{
			get
			{
				return this.oxymoronEngine;
			}
			private set
			{
				this.oxymoronEngine = value;
			}
		}

		#endregion

		#region Methods/Operators

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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.OxymoronEngine != null)
				this.OxymoronEngine.Dispose();

			this.OxymoronEngine = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			ResolveDictionaryValueDelegate resolveDictionaryValueCallback;
			IObfuscationSpec obfuscationSpec;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			resolveDictionaryValueCallback = (spec, key) => key;
			obfuscationSpec = new __XX();

			this.OxymoronEngine = new OxymoronEngine(resolveDictionaryValueCallback, obfuscationSpec);
		}

		protected override IChannel ProcessRecord(IContext context, RecordConfiguration recordConfiguration, IChannel channel, ProcessDelegate next)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			// simply wrap
			//channel.ApplyWrap(this.OxymoronEngine.GetObfuscatedValues);

			return next(context, recordConfiguration, channel);
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		public class __XX : IObfuscationSpec
		{
			#region Properties/Indexers/Events

			public IEnumerable<IDictionarySpec> DictionarySpecs
			{
				get;
				set;
			}

			public bool? DisableEngineCaches
			{
				get;
				set;
			}

			public bool? EnablePassThru
			{
				get;
				set;
			}

			public IHashSpec HashSpec
			{
				get;
				set;
			}

			public ITableSpec TableSpec
			{
				get;
				set;
			}

			#endregion

			#region Methods/Operators

			public void AssertValid()
			{
			}

			#endregion
		}

		#endregion
	}
}