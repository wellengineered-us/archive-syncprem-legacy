/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Primitives
{
	public class Field : IField
	{
		#region Constructors/Destructors

		public Field()
		{
		}

		#endregion

		#region Fields/Constants

		private long fieldIndex;
		private string fieldName;
		private Type fieldType;
		private bool isFieldKeyComponent;
		private bool isFieldOptional;

		#endregion

		#region Properties/Indexers/Events

		public long FieldIndex
		{
			get
			{
				return this.fieldIndex;
			}
			set
			{
				this.fieldIndex = value;
			}
		}

		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
			set
			{
				this.fieldName = value;
			}
		}

		public Type FieldType
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

		public bool IsFieldKeyComponent
		{
			get
			{
				return this.isFieldKeyComponent;
			}
			set
			{
				this.isFieldKeyComponent = value;
			}
		}

		public bool IsFieldOptional
		{
			get
			{
				return this.isFieldOptional;
			}
			set
			{
				this.isFieldOptional = value;
			}
		}

		#endregion
	}
}