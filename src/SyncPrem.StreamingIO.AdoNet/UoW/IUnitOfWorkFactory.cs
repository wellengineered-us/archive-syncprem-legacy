/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data;

namespace SyncPrem.StreamingIO.AdoNet.UoW
{
	public interface IUnitOfWorkFactory
	{
		#region Methods/Operators

		IUnitOfWork GetUnitOfWork(bool transactional, IsolationLevel isolationLevel = IsolationLevel.Unspecified);

		#endregion
	}
}