/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

		protected override Task PostExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void PostExecuteInternal(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)this.OxymoronEngine != null)
				this.OxymoronEngine.Dispose();

			this.OxymoronEngine = null;
		}

		protected override Task PreExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void PreExecuteInternal(IContext context, RecordConfiguration configuration)
		{
			ResolveDictionaryValueDelegate resolveDictionaryValueCallback;
			IObfuscationSpec obfuscationSpec;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			resolveDictionaryValueCallback = (spec, key) => key;
			obfuscationSpec = new __XX();

			this.OxymoronEngine = new OxymoronEngine(resolveDictionaryValueCallback, obfuscationSpec);
		}

		protected override Task<IChannel> ProcessAsyncInternal(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override IChannel ProcessInternal(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			// simply wrap
			//channel.ApplyWrap(this.OxymoronEngine.GetObfuscatedValues);

			return next(context, configuration, channel);
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