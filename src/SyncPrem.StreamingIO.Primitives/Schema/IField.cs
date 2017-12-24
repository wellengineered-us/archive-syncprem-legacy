/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public interface IField
	{
		#region Properties/Indexers/Events

		long FieldIndex
		{
			get;
		}

		string FieldName
		{
			get;
		}

		ISchema Schema
		{
			get;
		}

		#endregion
	}
}