/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Textual
{
	public abstract class TextualFieldSpec : ITextualFieldSpec
	{
		#region Constructors/Destructors

		protected TextualFieldSpec()
		{
		}

		#endregion

		#region Fields/Constants

		private long fieldOrdinal;
		private string fieldTitle;
		private string fieldFormat;
		private TextualFieldType fieldType;
		private bool isFieldIdentity;
		private bool isFieldRequired;

		#endregion

		#region Properties/Indexers/Events

		public long FieldOrdinal
		{
			get
			{
				return this.fieldOrdinal;
			}
			set
			{
				this.fieldOrdinal = value;
			}
		}

		public string FieldFormat
		{
			get
			{
				return this.fieldFormat;
			}
			set
			{
				this.fieldFormat = value;
			}
		}

		public string FieldTitle
		{
			get
			{
				return this.fieldTitle;
			}
			set
			{
				this.fieldTitle = value;
			}
		}

		public TextualFieldType FieldType
		{
			get
			{
				return this.fieldType;
			}
			set
			{
				this.fieldType = value;
			}
		}

		public bool IsFieldIdentity
		{
			get
			{
				return this.isFieldIdentity;
			}
			set
			{
				this.isFieldIdentity = value;
			}
		}

		public bool IsFieldRequired
		{
			get
			{
				return this.isFieldRequired;
			}
			set
			{
				this.isFieldRequired = value;
			}
		}

		#endregion
	}
}