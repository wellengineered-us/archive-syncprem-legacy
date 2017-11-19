/*
	Copyright �2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public interface IField
	{
		#region Properties/Indexers/Events

		object ContextData
		{
			get;
		}

		int FieldIndex
		{
			get;
		}

		string FieldName
		{
			get;
		}

		Type FieldType
		{
			get;
		}

		bool IsFieldOptional
		{
			get;
		}

		#endregion
	}
}