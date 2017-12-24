/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.StreamingIO.FlatText.Delimited
{
	public interface IDelimitedTextSpec : IFlatTextSpec
	{
		#region Properties/Indexers/Events

		string CloseQuoteValue
		{
			get;
			set;
		}

		IEnumerable<IDelimitedTextFieldSpec> DelimitedTextFieldSpecs
		{
			get;
			set;
		}

		string FieldDelimiter
		{
			get;
			set;
		}

		string OpenQuoteValue
		{
			get;
			set;
		}

		#endregion
	}
}