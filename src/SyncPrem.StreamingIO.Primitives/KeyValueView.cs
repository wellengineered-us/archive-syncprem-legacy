/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;

namespace SyncPrem.StreamingIO.Primitives
{
	public class KeyValueView : IKeyValueView
	{
		#region Constructors/Destructors

		public KeyValueView(ISchema originalSchema, IPayload originalPayload)
		{
			IPayload key, value;
			ISchemaBuilder k, v;

			if ((object)originalSchema == null)
				throw new ArgumentNullException(nameof(originalSchema));

			if ((object)originalPayload == null)
				throw new ArgumentNullException(nameof(originalPayload));

			var groups = originalSchema.Fields.Values.OrderBy(f => f.FieldIndex).GroupBy(f => f.IsFieldKeyComponent);

			key = new Payload();
			value = new Payload();
			k = new SchemaBuilder();
			v = new SchemaBuilder();

			foreach (IGrouping<bool, IField> grouping in groups)
			{
				foreach (IField field in grouping)
				{
					originalPayload.TryGetValue(field.FieldName, out object fieldValue);

					(grouping.Key ? key : value).Add(field.FieldName, fieldValue);
					(grouping.Key ? k : v).AddField(field.FieldName, field.FieldType, field.IsFieldOptional, field.IsFieldKeyComponent, field.FieldSchema);
				}
			}

			this.originalSchema = originalSchema;
			this.originalPayload = originalPayload;
			this.keyPayload = key;
			this.valuePayload = value;
			this.keySchema = k.Build();
			this.valueSchema = v.Build();
		}

		public KeyValueView(ISchema originalSchema, IPayload originalPayload,
			ISchema keySchema, IPayload keyPayload,
			ISchema valueSchema, IPayload valuePayload)
		{
			if ((object)originalSchema == null)
				throw new ArgumentNullException(nameof(originalSchema));

			if ((object)originalPayload == null)
				throw new ArgumentNullException(nameof(originalPayload));

			if ((object)keySchema == null)
				throw new ArgumentNullException(nameof(keySchema));

			if ((object)keyPayload == null)
				throw new ArgumentNullException(nameof(keyPayload));

			if ((object)valueSchema == null)
				throw new ArgumentNullException(nameof(keySchema));

			if ((object)valuePayload == null)
				throw new ArgumentNullException(nameof(valuePayload));

			this.originalSchema = originalSchema;
			this.originalPayload = originalPayload;
			this.keySchema = keySchema;
			this.keyPayload = keyPayload;
			this.valueSchema = valueSchema;
			this.valuePayload = valuePayload;
		}

		#endregion

		#region Fields/Constants

		private readonly IPayload keyPayload;
		private readonly ISchema keySchema;
		private readonly IPayload originalPayload;
		private readonly ISchema originalSchema;
		private readonly IPayload valuePayload;
		private readonly ISchema valueSchema;

		#endregion

		#region Properties/Indexers/Events

		public IPayload KeyPayload
		{
			get
			{
				return this.keyPayload;
			}
		}

		public ISchema KeySchema
		{
			get
			{
				return this.keySchema;
			}
		}

		public IPayload OriginalPayload
		{
			get
			{
				return this.originalPayload;
			}
		}

		public ISchema OriginalSchema
		{
			get
			{
				return this.originalSchema;
			}
		}

		public IPayload ValuePayload
		{
			get
			{
				return this.valuePayload;
			}
		}

		public ISchema ValueSchema
		{
			get
			{
				return this.valueSchema;
			}
		}

		#endregion
	}
}