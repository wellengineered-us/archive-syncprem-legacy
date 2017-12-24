/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public interface IArraySchema : ISchema
	{
		#region Properties/Indexers/Events

		ISchema ValueSchema
		{
			get;
		}

		#endregion
	}
}