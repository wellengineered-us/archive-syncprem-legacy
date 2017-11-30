/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public class Field : IField
	{
		#region Constructors/Destructors

		public Field(int fieldIndex)
		{
			this.fieldIndex = fieldIndex;
		}

		#endregion

		#region Fields/Constants

		private object contextData;
		private int fieldIndex;
		private string fieldName;
		private Type fieldType;
		private bool isFieldKeyComponent;
		private bool isFieldOptional;

		#endregion

		#region Properties/Indexers/Events

		public object ContextData
		{
			get
			{
				return this.contextData;
			}
			set
			{
				this.contextData = value;
			}
		}

		public int FieldIndex
		{
			get
			{
				return this.fieldIndex;
			}
			private set
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