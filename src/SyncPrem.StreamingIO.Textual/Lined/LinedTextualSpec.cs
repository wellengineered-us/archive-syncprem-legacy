/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.Textual.Lined
{
	public class LinedTextualSpec : TextualSpec<ILinedTextualFieldSpec>, ILinedTextualSpec
	{
		#region Constructors/Destructors

		public LinedTextualSpec()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public override string RecordDelimiter
		{
			get
			{
				return Environment.NewLine;
			}
			set
			{
				base.RecordDelimiter = Environment.NewLine;
			}
		}

		#endregion

		#region Methods/Operators

		public override void AssertValid()
		{
			IList<string> strings;

			strings = new List<string>();

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.RecordDelimiter))
				strings.Add(this.RecordDelimiter);

			if (strings.GroupBy(s => s).Where(gs => gs.Count() > 1).Any())
				throw new InvalidOperationException(string.Format("Duplicate delimiter/value encountered."));
		}

		#endregion
	}
}