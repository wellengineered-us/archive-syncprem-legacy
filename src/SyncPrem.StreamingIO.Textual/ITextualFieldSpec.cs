/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Textual
{
	public interface ITextualFieldSpec
	{
		#region Properties/Indexers/Events

		long FieldOrdinal
		{
			get;
		}

		TextualFieldType FieldType
		{
			get;
		}

		bool IsFieldIdentity
		{
			get;
		}

		bool IsFieldRequired
		{
			get;
		}

		string FieldTitle
		{
			get;
			set;
		}

		string FieldFormat
		{
			get;
		}

		#endregion
	}
}