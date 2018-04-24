﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Pipeline.Abstractions.Sync.Runtime
{
	public interface ISyncContextFactory
	{
		#region Methods/Operators

		ISyncContext CreateContext();

		#endregion
	}
}