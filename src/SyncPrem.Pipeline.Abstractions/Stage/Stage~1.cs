/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;

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

		private StageConfiguration<TStageSpecificConfiguration> stageConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public override Type StageSpecificConfigurationType
		{
			get
			{
				return typeof(TStageSpecificConfiguration);
			}
		}

		public new StageConfiguration<TStageSpecificConfiguration> StageConfiguration
		{
			get
			{
				return this.stageConfiguration;
			}
			private set
			{
				this.stageConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void Create(bool creating)
		{
			StageConfiguration baseUntypedStageConfiguration;
			StageConfiguration<TStageSpecificConfiguration> _stageConfiguration;

			base.Create(creating);

			if (!creating)
				return;

			baseUntypedStageConfiguration = base.StageConfiguration;

			if ((object)baseUntypedStageConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(baseUntypedStageConfiguration)));

			if ((object)baseUntypedStageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(baseUntypedStageConfiguration.StageSpecificConfiguration)));

			_stageConfiguration = new StageConfiguration<TStageSpecificConfiguration>(baseUntypedStageConfiguration);

			if ((object)_stageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(_stageConfiguration.StageSpecificConfiguration)));

			this.StageConfiguration = _stageConfiguration;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StageConfiguration = null;
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}