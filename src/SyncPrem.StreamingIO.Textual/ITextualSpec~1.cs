/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Textual
{
	public interface ITextualSpec<TTextualFieldSpec>
		where TTextualFieldSpec : ITextualFieldSpec
	{
		#region Properties/Indexers/Events

		bool IsFirstRecordHeader
		{
			get;
		}

		bool IsLastRecordFooter
		{
			get;
		}

		string RecordDelimiter
		{
			get;
		}

		IList<TTextualFieldSpec> TextualFooterSpecs
		{
			get;
		}

		IList<TTextualFieldSpec> TextualHeaderSpecs
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void AssertValid();

		#endregion
	}
}