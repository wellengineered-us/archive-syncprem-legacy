/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron
{
	public interface IOxymoronEngine : IDisposable
	{
		#region Methods/Operators

		object GetObfuscatedValue(IField field, object originalFieldValue);

		IEnumerable<IRecord> GetObfuscatedValues(IEnumerable<IRecord> records);

		#endregion
	}
}