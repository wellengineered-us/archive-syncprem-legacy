/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Textual
{
	public interface ITextualStreamingRecord : IPayload
	{
		#region Properties/Indexers/Events

		long CharacterNumber
		{
			get;
		}

		long LineNumber
		{
			get;
		}

		long RecordIndex
		{
			get;
		}

		#endregion
	}
}