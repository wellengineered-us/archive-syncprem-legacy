/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage
{
	public abstract class AsyncStage<TStageSpecificConfiguration> : AsyncStage
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected AsyncStage()
		{
		}

		#endregion

		#region Fields/Constants

		private StageConfiguration<TStageSpecificConfiguration> configuration;

		#endregion

		#region Properties/Indexers/Events

		public override IAsyncValidatable StageSpecificAsyncValidatable
		{
			get
			{
				return this?.Configuration?.StageSpecificConfiguration;
			}
		}

		public override Type StageSpecificConfigurationType
		{
			get
			{
				return typeof(TStageSpecificConfiguration);
			}
		}

		public new StageConfiguration<TStageSpecificConfiguration> Configuration
		{
			get
			{
				return this.configuration;
			}
			private set
			{
				this.configuration = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected void AssertValidConfiguration()
		{
			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.Configuration)));

			if ((object)this.Configuration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.Configuration.StageSpecificConfiguration)));
		}

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			StageConfiguration untypedStageConfiguration;
			StageConfiguration<TStageSpecificConfiguration> typedStageConfiguration;

			await base.CreateAsync(creating, cancellationToken);

			if (!creating)
				return;

			untypedStageConfiguration = base.Configuration;

			if ((object)untypedStageConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(untypedStageConfiguration)));

			if ((object)untypedStageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(untypedStageConfiguration.StageSpecificConfiguration)));

			typedStageConfiguration = new StageConfiguration<TStageSpecificConfiguration>(untypedStageConfiguration);

			if ((object)typedStageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(typedStageConfiguration.StageSpecificConfiguration)));

			this.Configuration = typedStageConfiguration;

			this.AssertValidConfiguration();
		}

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			if (disposing)
			{
				this.Configuration = null;
			}

			await base.DisposeAsync(disposing, cancellationToken);
		}

		#endregion
	}
}