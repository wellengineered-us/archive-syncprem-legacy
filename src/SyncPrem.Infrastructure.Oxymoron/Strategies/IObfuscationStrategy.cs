/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies
{
	public interface IObfuscationStrategy
	{
		#region Methods/Operators

		object GetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec columnSpec, IField field, object originalFieldValue);

		Type GetObfuscationStrategySpecificSpecType();

		#endregion
	}
}