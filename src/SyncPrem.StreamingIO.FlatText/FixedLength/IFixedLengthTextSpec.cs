/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.StreamingIO.FlatText.FixedLength
{
	public interface IFixedLengthTextSpec : IFlatTextSpec
	{
		#region Properties/Indexers/Events

		IEnumerable<IFixedLengthTextFieldSpec> DelimitedTextFieldSpecs
		{
			get;
			set;
		}

		int RecordLength
		{
			get;
			set;
		}

		#endregion
	}
}