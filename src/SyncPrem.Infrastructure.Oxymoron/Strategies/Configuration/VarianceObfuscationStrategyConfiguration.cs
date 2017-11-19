/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration
{
	public class VarianceObfuscationStrategyConfiguration : ObfuscationStrategyConfiguration
	{
		#region Constructors/Destructors

		public VarianceObfuscationStrategyConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private long? variancePercentValue;

		#endregion

		#region Properties/Indexers/Events

		public long? VariancePercentValue
		{
			get
			{
				return this.variancePercentValue;
			}
			set
			{
				this.variancePercentValue = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int? columnIndex;

			columnIndex = context as int?;
			messages = new List<Message>();

			if ((object)this.VariancePercentValue == null)
				messages.Add(NewError(string.Format("Column[{0}/{1}] variance percent value is required.", columnIndex, this.Parent.ColumnName)));
			else if (!((int)this.VariancePercentValue >= -100 && (int)this.VariancePercentValue <= 100))
				messages.Add(NewError(string.Format("Column[{0}/{1}] variance percent value must be between -100 and +100.", columnIndex, this.Parent.ColumnName)));

			return messages;
		}

		#endregion
	}
}