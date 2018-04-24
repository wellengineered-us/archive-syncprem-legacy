/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.Textual.Delimited
{
	public class DelimitedTextualSpec : TextualSpec<IDelimitedTextualFieldSpec>, IDelimitedTextualSpec
	{
		#region Constructors/Destructors

		public DelimitedTextualSpec()
		{
		}

		#endregion

		#region Fields/Constants

		private string closeQuoteValue;
		private string fieldDelimiter;
		private string openQuoteValue;

		#endregion

		#region Properties/Indexers/Events

		public string CloseQuoteValue
		{
			get
			{
				return this.closeQuoteValue;
			}
			set
			{
				this.closeQuoteValue = value;
			}
		}

		public string FieldDelimiter
		{
			get
			{
				return this.fieldDelimiter;
			}
			set
			{
				this.fieldDelimiter = value;
			}
		}

		public string OpenQuoteValue
		{
			get
			{
				return this.openQuoteValue;
			}
			set
			{
				this.openQuoteValue = value;
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

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.FieldDelimiter))
				strings.Add(this.FieldDelimiter);

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.OpenQuoteValue) &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.CloseQuoteValue))
			{
				if (this.OpenQuoteValue == this.CloseQuoteValue)
					strings.Add(this.OpenQuoteValue + this.CloseQuoteValue);
				else
				{
					strings.Add(this.OpenQuoteValue);
					strings.Add(this.CloseQuoteValue);
				}
			}

			if (strings.GroupBy(s => s).Where(gs => gs.Count() > 1).Any())
				throw new InvalidOperationException(string.Format("Duplicate delimiter/value encountered."));
		}

		#endregion
	}
}