/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Stage
{
	public abstract class Stage<TStageSpecificConfiguration> : Stage
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected Stage()
		{
		}

		#endregion

		#region Fields/Constants

		private StageConfiguration<TStageSpecificConfiguration> configuration;

		#endregion

		#region Properties/Indexers/Events

		public override Type StageSpecificConfigurationType
		{
			get
			{
				return typeof(TStageSpecificConfiguration);
			}
		}

		public override IValidatable StageSpecificValidatable
		{
			get
			{
				return this?.Configuration?.StageSpecificConfiguration;
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

		protected override void Create(bool creating)
		{
			StageConfiguration untypedStageConfiguration;
			StageConfiguration<TStageSpecificConfiguration> typedStageConfiguration;

			base.Create(creating);

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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Configuration = null;
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}