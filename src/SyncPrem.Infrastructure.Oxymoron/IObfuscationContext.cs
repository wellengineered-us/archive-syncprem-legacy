﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Infrastructure.Oxymoron.Configuration;

namespace SyncPrem.Infrastructure.Oxymoron
{
	public interface IObfuscationContext
	{
		#region Methods/Operators

		long GetSignHash(object value);

		long GetValueHash(long? size, object value);

		bool TryGetSurrogateValue(DictionaryConfiguration dictionaryConfiguration, object surrogateKey, out object surrogateValue);

		#endregion
	}
}