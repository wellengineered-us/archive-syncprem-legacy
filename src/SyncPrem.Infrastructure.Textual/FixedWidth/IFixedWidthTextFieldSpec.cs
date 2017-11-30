/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Infrastructure.Textual.FixedWidth
{
	public interface IFixedWidthTextFieldSpec : IFlatTextFieldSpec
	{
		#region Properties/Indexers/Events

		int FieldWidth
		{
			get;
			set;
		}

		int StartPosition
		{
			get;
			set;
		}

		#endregion
	}
}