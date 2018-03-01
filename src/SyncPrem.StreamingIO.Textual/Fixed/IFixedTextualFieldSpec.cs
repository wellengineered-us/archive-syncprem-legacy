/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Textual.Fixed
{
	public interface IFixedTextualFieldSpec : ITextualFieldSpec
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