/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Textual
{
	public interface IFlatTextSpec
	{
		#region Properties/Indexers/Events

		IEnumerable<IFlatTextFieldSpec> FlatTextFieldSpecs
		{
			get;
		}

		bool? FirstRecordIsHeader
		{
			get;
			set;
		}

		bool? LastRecordIsFooter
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