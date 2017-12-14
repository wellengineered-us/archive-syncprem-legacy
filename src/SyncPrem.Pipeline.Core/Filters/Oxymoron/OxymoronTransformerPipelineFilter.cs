﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Oxymoron;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Transformer;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Filters.Oxymoron
{
	public class OxymoronTransformerPipelineFilter : TransformerPipelineFilter<FilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public OxymoronTransformerPipelineFilter()
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

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.OxymoronEngine != null)
				this.OxymoronEngine.Dispose();

			this.OxymoronEngine = null;
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			ResolveDictionaryValueDelegate resolveDictionaryValueCallback;
			IObfuscationSpec obfuscationSpec;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			resolveDictionaryValueCallback = (spec, key) => key;
			obfuscationSpec = new __XX();

			this.OxymoronEngine = new OxymoronEngine(resolveDictionaryValueCallback, obfuscationSpec);
		}

		protected override IPipelineMessage TransformMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			// simply wrap
			pipelineMessage.ApplyWrap(this.OxymoronEngine.GetObfuscatedValues);

			return next(pipelineContext, tableConfiguration, pipelineMessage);
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