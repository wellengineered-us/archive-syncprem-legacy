/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Stage
{
	public abstract class Stage<TStageSpecificConfiguration> : Stage, ISpecifiable<TStageSpecificConfiguration>
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected Stage()
		{
		}

		#endregion

		#region Fields/Constants

		private TStageSpecificConfiguration specification;

		#endregion

		#region Properties/Indexers/Events

		public Type SpecificationType
		{
			get
			{
				return typeof(TStageSpecificConfiguration);
			}
		}

		public override Type StageSpecificConfigurationType
		{
			get
			{
				return this.SpecificationType;
			}
		}

		public TStageSpecificConfiguration Specification
		{
			get
			{
				return this.specification;
			}
			set
			{
				this.specification = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void Create(bool creating)
		{
			StageConfiguration untypedStageConfiguration;
			StageConfiguration<TStageSpecificConfiguration> typedStageConfiguration;

			base.Create(creating);

			if (!creating)
				return;

			untypedStageConfiguration = this.Configuration;

			if ((object)untypedStageConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(untypedStageConfiguration)));

			if ((object)untypedStageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(untypedStageConfiguration.StageSpecificConfiguration)));

			typedStageConfiguration = new StageConfiguration<TStageSpecificConfiguration>(untypedStageConfiguration);

			if ((object)typedStageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(typedStageConfiguration.StageSpecificConfiguration)));

			this.Specification = typedStageConfiguration.StageSpecificConfiguration;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Configuration = null;
				this.Specification = null;
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}