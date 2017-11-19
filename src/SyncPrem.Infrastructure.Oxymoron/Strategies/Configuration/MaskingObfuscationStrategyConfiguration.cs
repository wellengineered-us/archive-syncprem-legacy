/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration
{
	public class MaskingObfuscationStrategyConfiguration : ObfuscationStrategyConfiguration
	{
		#region Constructors/Destructors

		public MaskingObfuscationStrategyConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private long? maskingPercentValue;

		#endregion

		#region Properties/Indexers/Events

		public long? MaskingPercentValue
		{
			get
			{
				return this.maskingPercentValue;
			}
			set
			{
				this.maskingPercentValue = value;
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

			if ((object)this.MaskingPercentValue == null)
				messages.Add(NewError(string.Format("Column[{0}/{1}] masking percent value is required.", columnIndex, this.Parent.ColumnName)));
			else if (!((int)this.MaskingPercentValue >= -100 && (int)this.MaskingPercentValue <= 100))
				messages.Add(NewError(string.Format("Column[{0}/{1}] masking percent value must be between -100 and +100.", columnIndex, this.Parent.ColumnName)));

			return messages;
		}

		#endregion
	}
}