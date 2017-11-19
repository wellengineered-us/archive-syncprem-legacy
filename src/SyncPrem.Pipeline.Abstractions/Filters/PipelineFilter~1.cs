/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;

namespace SyncPrem.Pipeline.Abstractions.Filters
{
	public abstract class PipelineFilter<TFilterSpecificConfiguration> : PipelineFilter
		where TFilterSpecificConfiguration : FilterSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected PipelineFilter()
		{
		}

		#endregion

		#region Fields/Constants

		private FilterConfiguration<TFilterSpecificConfiguration> filterConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public override Type FilterSpecificConfigurationType
		{
			get
			{
				return typeof(TFilterSpecificConfiguration);
			}
		}

		public new FilterConfiguration<TFilterSpecificConfiguration> FilterConfiguration
		{
			get
			{
				return this.filterConfiguration;
			}
			private set
			{
				this.filterConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void Create(bool creating)
		{
			FilterConfiguration baseUntypedFilterConfiguration;
			FilterConfiguration<TFilterSpecificConfiguration> _filterConfiguration;

			base.Create(creating);

			if (!creating)
				return;

			baseUntypedFilterConfiguration = base.FilterConfiguration;

			if ((object)baseUntypedFilterConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(baseUntypedFilterConfiguration)));

			if ((object)baseUntypedFilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(baseUntypedFilterConfiguration.FilterSpecificConfiguration)));

			_filterConfiguration = new FilterConfiguration<TFilterSpecificConfiguration>(baseUntypedFilterConfiguration);

			if ((object)_filterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(_filterConfiguration.FilterSpecificConfiguration)));

			this.FilterConfiguration = _filterConfiguration;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.FilterConfiguration = null;
			}

			base.Dispose(disposing);
		}

		#endregion

		//protected IEnumerable<Message> ValidateConfiguration(FilterConfiguration filterConfiguration, object context)
		//{
		//	FilterConfiguration<TFilterSpecificConfiguration> _filterConfiguration;

		//	if ((object)filterConfiguration == null)
		//		throw new ArgumentNullException(nameof(filterConfiguration));

		//	if ((object)filterConfiguration.FilterSpecificConfiguration == null)
		//		throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(filterConfiguration.FilterSpecificConfiguration)));

		//	_filterConfiguration = new FilterConfiguration<TFilterSpecificConfiguration>(filterConfiguration);

		//	if ((object)_filterConfiguration.FilterSpecificConfiguration == null)
		//		throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(_filterConfiguration.FilterSpecificConfiguration)));

		//	return _filterConfiguration.FilterSpecificConfiguration.Validate(context);
		//}
	}
}