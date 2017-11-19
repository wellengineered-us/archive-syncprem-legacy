/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Oxymoron.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies
{
	public interface IObfuscationStrategy
	{
		#region Methods/Operators

		object GetObfuscatedValue(IObfuscationContext obfuscationContext, ColumnConfiguration columnConfiguration, IField field, object originalFieldValue);

		Type GetObfuscationStrategySpecificConfigurationType();

		IEnumerable<Message> ValidateObfuscationStrategySpecificConfiguration(ColumnConfiguration columnConfiguration, int? fieldIndex);

		#endregion
	}
}