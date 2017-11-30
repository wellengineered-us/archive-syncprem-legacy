/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Infrastructure.Textual.Delimited
{
	public class DelimitedTextFieldSpec : FlatTextFieldSpec, IDelimitedTextFieldSpec
	{
		#region Constructors/Destructors

		public DelimitedTextFieldSpec(int fieldIndex)
			: base(fieldIndex)
		{
		}

		#endregion
	}
}