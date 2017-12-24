/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.StreamingIO.Primitives.Schema;

namespace SyncPrem.StreamingIO.Primitives.Value
{
	public interface IValue
	{
		#region Properties/Indexers/Events

		ISchema Schema
		{
			get;
		}

		#endregion
	}
}