/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Textual.FixedWidth
{
	public interface IFixedWidthTextSpec : IFlatTextSpec
	{
		#region Properties/Indexers/Events

		IEnumerable<IFixedWidthTextFieldSpec> DelimitedTextFieldSpecs
		{
			get;
			set;
		}

		int? RecordWidth
		{
			get;
			set;
		}

		#endregion
	}
}