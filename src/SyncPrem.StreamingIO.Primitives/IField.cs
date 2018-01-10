/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Primitives
{
	public interface IField
	{
		#region Properties/Indexers/Events

		long FieldIndex
		{
			get;
		}

		Type FieldType
		{
			get;
		}

		bool IsKeyComponent
		{
			get;
		}

		bool IsOptional
		{
			get;
		}

		ISchema Schema
		{
			get;
		}

		string FieldName
		{
			get;
			set;
		}

		#endregion
	}
}