/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public class Field : IField
	{
		#region Constructors/Destructors

		public Field(long fieldIndex, string fieldName, ISchema fieldSchema)
		{
			this.fieldIndex = fieldIndex;
			this.fieldName = fieldName;
			this.schema = fieldSchema;
		}

		#endregion

		#region Fields/Constants

		private readonly long fieldIndex;
		private readonly string fieldName;
		private readonly ISchema schema;

		#endregion

		#region Properties/Indexers/Events

		public long FieldIndex
		{
			get
			{
				return this.fieldIndex;
			}
		}

		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
		}

		public ISchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		#endregion
	}
}