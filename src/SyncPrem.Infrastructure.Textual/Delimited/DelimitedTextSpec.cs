/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.Infrastructure.Textual.Delimited
{
	public class DelimitedTextSpec : FlatTextSpec, IDelimitedTextSpec
	{
		#region Constructors/Destructors

		public DelimitedTextSpec()
		{
		}

		#endregion

		#region Fields/Constants

		private string closeQuoteValue;

		private IEnumerable<IDelimitedTextFieldSpec> delimitedTextFieldSpecs;
		private string fieldDelimiter;
		private string openQuoteValue;

		#endregion

		#region Properties/Indexers/Events

		public override IEnumerable<IFlatTextFieldSpec> FlatTextFieldSpecs
		{
			get
			{
				return this.DelimitedTextFieldSpecs;
			}
		}

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

		public IEnumerable<IDelimitedTextFieldSpec> DelimitedTextFieldSpecs
		{
			get
			{
				return this.delimitedTextFieldSpecs;
			}
			set
			{
				this.delimitedTextFieldSpecs = value;
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

			if (!string.IsNullOrEmpty(this.RecordDelimiter))
				strings.Add(this.RecordDelimiter);

			if (!string.IsNullOrEmpty(this.FieldDelimiter))
				strings.Add(this.FieldDelimiter);

			if (!string.IsNullOrEmpty(this.OpenQuoteValue))
				strings.Add(this.OpenQuoteValue);

			if (!string.IsNullOrEmpty(this.CloseQuoteValue))
				strings.Add(this.CloseQuoteValue);

			if (strings.GroupBy(s => s).Where(gs => gs.Count() > 1).Any())
				throw new InvalidOperationException(string.Format("Duplicate delimiter/value encountered."));
		}

		#endregion
	}
}