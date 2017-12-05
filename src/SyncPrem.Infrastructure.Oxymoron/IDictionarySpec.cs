/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Infrastructure.Oxymoron
{
	public interface IDictionarySpec
	{
		#region Properties/Indexers/Events

		string DictionaryId
		{
			get;
			set;
		}

		bool PreloadEnabled
		{
			get;
			set;
		}

		long? RecordCount
		{
			get;
			set;
		}

		#endregion
	}
}