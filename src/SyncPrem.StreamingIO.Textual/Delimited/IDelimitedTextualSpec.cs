/*
	Copyright ?2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Textual.Delimited
{
	public interface IDelimitedTextualSpec : ITextualSpec<IDelimitedTextualFieldSpec>
	{
		#region Properties/Indexers/Events

		string CloseQuoteValue
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