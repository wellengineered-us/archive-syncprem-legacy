/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.DataMasking
{
	public interface IOxymoronEngine : IDisposable
	{
		#region Methods/Operators

		object GetObfuscatedValue(IField field, object originalFieldValue);

		IEnumerable<IPayload> GetObfuscatedValues(IEnumerable<IPayload> records);

		#endregion
	}
}