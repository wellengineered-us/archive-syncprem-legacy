/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.StreamingIO.Primitives
{
	public sealed class SchemaBuilder : ISchemaBuilder, ISchema
	{
		#region Constructors/Destructors

		public SchemaBuilder()
			: this(new Dictionary<string, IField>(StringComparer.OrdinalIgnoreCase))
		{
		}

		public SchemaBuilder(Dictionary<string, IField> fields)
		{
			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			this.fields = fields;
		}

		#endregion

		#region Fields/Constants

		private static readonly ISchema empty = Create().WithVersion(0).Build();
		private readonly Dictionary<string, IField> fields;
		private string schemaName;
		private SchemaType schemaType;
		private int schemaVersion;

		#endregion

		#region Properties/Indexers/Events

		public static ISchema Empty
		{
			get
			{
				return empty;
			}
		}

		public IReadOnlyDictionary<string, IField> Fields
		{
			get
			{
				return this.fields;
			}
		}

		private IDictionary<string, IField> MutableFields
		{
			get
			{
				return this.fields;
			}
		}

		public ISchema Schema
		{
			get
			{
				return this.Build();
			}
		}

		public string SchemaName
		{
			get
			{
				return this.schemaName;
			}
			private set
			{
				this.schemaName = value;
			}
		}

		public SchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
			private set
			{
				this.schemaType = value;
			}
		}

		public int SchemaVersion
		{
			get
			{
				return this.schemaVersion;
			}
			private set
			{
				this.schemaVersion = value;
			}
		}

		#endregion

		#region Methods/Operators

		private static void AssertCanSet(string propertyName, object propertyValue, object newValue)
		{
			if (propertyValue != null && propertyValue != newValue)
				throw new InvalidOperationException(string.Format("SchemaBuilder: Property '{0}' has already been set.", propertyName));
		}

		private static void AssertCanSet(string propertyName, bool propertyValue, bool newValue)
		{
			if (propertyValue != false && propertyValue != newValue)
				throw new InvalidOperationException(string.Format("SchemaBuilder: Property '{0}' has already been set.", propertyName));
		}

		private static void AssertCanSet(string propertyName, int propertyValue, int newValue)
		{
			if (propertyValue != 0 && propertyValue != newValue)
				throw new InvalidOperationException(string.Format("SchemaBuilder: Property '{0}' has already been set.", propertyName));
		}

		public static SchemaBuilder Create()
		{
			return new SchemaBuilder() { SchemaType = SchemaType.Object };
		}

		public static ISchema FromType(Type type)
		{
			SchemaBuilder schemaBuilder;

			if ((object)type == null)
				throw new ArgumentNullException(nameof(type));

			schemaBuilder = new SchemaBuilder();

			return schemaBuilder.Schema;
		}

		public SchemaBuilder AddField(string fieldName, Type fieldType, bool isFieldOptional, bool isFieldKeyPart, ISchema fieldSchema = null)
		{
			if ((object)fieldName == null)
				throw new ArgumentNullException(nameof(fieldName));

			if ((object)fieldType == null)
				throw new ArgumentNullException(nameof(fieldType));

			this.MutableFields.Add(fieldName, new Field()
											{
												FieldIndex = this.MutableFields.Count,
												FieldName = fieldName,
												FieldType = fieldType,
												IsFieldOptional = isFieldOptional,
												IsFieldKeyComponent = isFieldKeyPart,
												FieldSchema = fieldSchema
											});
			return this;
		}

		public SchemaBuilder AddFields(IEnumerable<IField> fields)
		{
			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			foreach (IField field in fields)
				this.MutableFields.Add(field.FieldName, field);

			return this;
		}

		public ISchema Build()
		{
			return this;
		}

		public override string ToString()
		{
			return string.Join(", ", this.Fields.Values.Select(f => $"[{f.FieldName}]@{f.FieldIndex}"));
		}

		public SchemaBuilder WithName(string value)
		{
			AssertCanSet(nameof(this.SchemaName), this.SchemaName, value);
			this.SchemaName = value;
			return this;
		}

		public SchemaBuilder WithVersion(int value)
		{
			AssertCanSet(nameof(this.SchemaVersion), this.SchemaVersion, value);
			this.SchemaVersion = value;
			return this;
		}

		public SchemaBuilder WithType(SchemaType value)
		{
			AssertCanSet(nameof(this.SchemaType), this.SchemaType, value);
			this.SchemaType = value;
			return this;
		}

		#endregion
	}
}