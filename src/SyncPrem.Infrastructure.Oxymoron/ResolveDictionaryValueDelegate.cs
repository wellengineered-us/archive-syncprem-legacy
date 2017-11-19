﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Infrastructure.Oxymoron.Configuration;

namespace SyncPrem.Infrastructure.Oxymoron
{
	public delegate object ResolveDictionaryValueDelegate(DictionaryConfiguration dictionaryConfiguration, object surrogateKey);
}