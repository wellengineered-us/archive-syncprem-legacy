/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Textual
{
	public abstract class TextualSpec<TTextualFieldSpec> : ITextualSpec<TTextualFieldSpec>
		where TTextualFieldSpec : ITextualFieldSpec
	{
		#region Constructors/Destructors

		protected TextualSpec()
			: this(new List<TTextualFieldSpec>(), new List<TTextualFieldSpec>())
		{
		}

		protected TextualSpec(IList<TTextualFieldSpec> textualHeaderSpecs, IList<TTextualFieldSpec> textualFooterSpecs)
		{
			if ((object)textualHeaderSpecs == null)
				throw new ArgumentNullException(nameof(textualHeaderSpecs));

			if ((object)textualFooterSpecs == null)
				throw new ArgumentNullException(nameof(textualFooterSpecs));

			this.textualHeaderSpecs = textualHeaderSpecs;
			this.textualFooterSpecs = textualFooterSpecs;
		}

		#endregion

		#region Fields/Constants

		private readonly IList<TTextualFieldSpec> textualFooterSpecs;
		private readonly IList<TTextualFieldSpec> textualHeaderSpecs;
		private bool isFirstRecordHeader;
		private bool isLastRecordFooter;
		private string recordDelimiter;

		#endregion

		#region Properties/Indexers/Events

		public IList<TTextualFieldSpec> TextualFooterSpecs
		{
			get
			{
				return this.textualFooterSpecs;
			}
		}

		public IList<TTextualFieldSpec> TextualHeaderSpecs
		{
			get
			{
				return this.textualHeaderSpecs;
			}
		}

		public bool IsFirstRecordHeader
		{
			get
			{
				return this.isFirstRecordHeader;
			}
			set
			{
				this.isFirstRecordHeader = value;
			}
		}

		public bool IsLastRecordFooter
		{
			get
			{
				return this.isLastRecordFooter;
			}
			set
			{
				this.isLastRecordFooter = value;
			}
		}

		public string RecordDelimiter
		{
			get
			{
				return this.recordDelimiter;
			}
			set
			{
				this.recordDelimiter = value;
			}
		}

		#endregion

		#region Methods/Operators

		public virtual void AssertValid()
		{
			// TODO
		}

		#endregion
	}
}