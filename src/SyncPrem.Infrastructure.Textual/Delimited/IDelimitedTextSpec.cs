/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Textual.Delimited
{
	public interface IDelimitedTextSpec
	{
		#region Properties/Indexers/Events

		List<Field> HeaderSpecs
		{
			get;
		}

		string FieldDelimiter
		{
			get;
			set;
		}

		bool? FirstRecordIsHeader
		{
			get;
			set;
		}

		string QuoteValue
		{
			get;
			set;
		}

		string RecordDelimiter
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		void AssertValid();

		#endregion
	}
}