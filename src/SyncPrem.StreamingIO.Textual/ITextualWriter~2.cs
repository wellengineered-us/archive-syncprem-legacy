/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;
using System.IO;

using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.StreamingIO.Textual
{
	public interface ITextualWriter<in TTextualFieldSpec, out TTextualSpec> : IDisposableEx
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Properties/Indexers/Events

		TextWriter BaseTextWriter
		{
			get;
		}

		TTextualSpec TextualSpec
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void Flush();

		void WriteFooterRecords(IEnumerable<TTextualFieldSpec> specs, IEnumerable<ITextualStreamingRecord> footers);

		void WriteHeaderFields(IEnumerable<TTextualFieldSpec> specs);

		void WriteRecords(IEnumerable<IPayload /* SHOULD BE LCD INTERFACE */> records);

		#endregion
	}
}