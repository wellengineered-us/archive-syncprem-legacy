/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Textual.Fixed
{
	public interface IFixedTextualSpec : ITextualSpec<IFixedTextualFieldSpec>
	{
		#region Properties/Indexers/Events

		int RecordLength
		{
			get;
			set;
		}

		#endregion
	}
}