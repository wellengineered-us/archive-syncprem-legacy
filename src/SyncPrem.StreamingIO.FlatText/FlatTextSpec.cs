/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.StreamingIO.FlatText
{
	public abstract class FlatTextSpec : IFlatTextSpec
	{
		#region Constructors/Destructors

		protected FlatTextSpec()
		{
		}

		#endregion

		#region Fields/Constants

		private bool firstRecordIsHeader;
		private bool lastRecordIsFooter;
		private string recordDelimiter;

		#endregion

		#region Properties/Indexers/Events

		public abstract IEnumerable<IFlatTextFieldSpec> FlatTextFieldSpecs
		{
			get;
		}

		public bool FirstRecordIsHeader
		{
			get
			{
				return this.firstRecordIsHeader;
			}
			set
			{
				this.firstRecordIsHeader = value;
			}
		}

		public bool LastRecordIsFooter
		{
			get
			{
				return this.lastRecordIsFooter;
			}
			set
			{
				this.lastRecordIsFooter = value;
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
			// do nothing
		}

		#endregion
	}
}