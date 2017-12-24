/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public interface IStructSchema : ISchema
	{
		#region Properties/Indexers/Events

		IReadOnlyList<IField> Fields
		{
			get;
		}

		#endregion

		#region Methods/Operators

		IField GetFieldByName(string fieldName);

		#endregion
	}
}