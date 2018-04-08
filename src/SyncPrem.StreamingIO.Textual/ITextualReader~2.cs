/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;
using System.IO;
using System.Threading;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.StreamingIO.Textual
{
	public interface ITextualReader<TTextualFieldSpec, out TTextualSpec> : IDisposableEx
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Properties/Indexers/Events

		TextReader BaseTextReader
		{
			get;
		}

		TTextualSpec TextualSpec
		{
			get;
		}

		#endregion

		#region Methods/Operators

		IEnumerable<ITextualStreamingRecord> ReadFooterRecords(IEnumerable<TTextualFieldSpec> footers);

		IAsyncEnumerable<ITextualStreamingRecord> ReadFooterRecordsAsync(IEnumerable<TTextualFieldSpec> footers, CancellationToken cancellationToken);

		IEnumerable<TTextualFieldSpec> ReadHeaderFields();

		IAsyncEnumerable<TTextualFieldSpec> ReadHeaderFieldsAsync(CancellationToken cancellationToken);

		IEnumerable<ITextualStreamingRecord> ReadRecords();

		IAsyncEnumerable<ITextualStreamingRecord> ReadRecordsAsync(CancellationToken cancellationToken);

		#endregion
	}
}