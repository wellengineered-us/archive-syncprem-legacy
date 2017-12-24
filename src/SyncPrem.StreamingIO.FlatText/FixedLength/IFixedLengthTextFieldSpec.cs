/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.FlatText.FixedLength
{
	public interface IFixedLengthTextFieldSpec : IFlatTextFieldSpec
	{
		#region Properties/Indexers/Events

		int FieldLength
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