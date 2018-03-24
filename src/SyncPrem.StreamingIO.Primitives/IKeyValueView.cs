/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Primitives
{
	public interface IKeyValueView
	{
		#region Properties/Indexers/Events

		IPayload KeyPayload
		{
			get;
		}

		ISchema KeySchema
		{
			get;
		}

		IPayload OriginalPayload
		{
			get;
		}

		ISchema OriginalSchema
		{
			get;
		}

		IPayload ValuePayload
		{
			get;
		}

		ISchema ValueSchema
		{
			get;
		}

		#endregion
	}
}