/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives
{
	public interface ISchema
	{
		#region Properties/Indexers/Events

		IReadOnlyDictionary<string, IField> Fields
		{
			get;
		}

		string SchemaName
		{
			get;
		}

		int SchemaVersion
		{
			get;
		}

		#endregion
	}
}