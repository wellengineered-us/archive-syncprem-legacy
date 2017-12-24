/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public sealed class SchemaBuilder : IAnySchema
	{
		#region Constructors/Destructors

		public SchemaBuilder(SchemaType schemaType)
		{
			//if ((object)schemaType == null)
			//throw new ArgumentNullException(nameof(schemaType));

			this.schemaType = schemaType;

			if (this.schemaType == SchemaType.Struct)
				this.fields = new List<IField>();
			else
				this.fields = null;
		}

		#endregion

		#region Fields/Constants

		private readonly List<IField> fields;
		private readonly Dictionary<string, string> parameters = new Dictionary<string, string>();
		private readonly SchemaType schemaType;
		private object defaultValue;
		private string documentation;
		private bool isIdentity;
		private bool isOptional;
		private bool isPrimitiveType;
		private ISchema keySchema;
		private string schemaName;
		private int schemaVersion;
		private ISchema valueSchema;

		#endregion

		#region Properties/Indexers/Events

		public IReadOnlyList<IField> Fields
		{
			get
			{
				return this.fields;
			}
		}

		private IList<IField> MutableFields
		{
			get
			{
				return this.fields;
			}
		}

		public IDictionary<string, string> MutableParameters
		{
			get
			{
				return this.parameters;
			}
		}

		public IReadOnlyDictionary<string, string> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		public ISchema Schema
		{
			get
			{
				return this.Build();
			}
		}

		public SchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
		}

		ISchema IMapSchema.ValueSchema
		{
			get
			{
				return this.valueSchema;
			}
		}

		ISchema IArraySchema.ValueSchema
		{
			get
			{
				return this.valueSchema;
			}
		}

		public object DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			private set
			{
				this.defaultValue = value;
			}
		}

		public string Documentation
		{
			get
			{
				return this.documentation;
			}
			private set
			{
				this.documentation = value;
			}
		}

		public bool IsIdentity
		{
			get
			{
				return this.isIdentity;
			}
			private set
			{
				this.isIdentity = value;
			}
		}

		public bool IsOptional
		{
			get
			{
				return this.isOptional;
			}
			private set
			{
				this.isOptional = value;
			}
		}

		public bool IsPrimitiveType
		{
			get
			{
				return this.isPrimitiveType;
			}
			private set
			{
				this.isPrimitiveType = value;
			}
		}

		public ISchema KeySchema
		{
			get
			{
				return this.keySchema;
			}
			private set
			{
				this.keySchema = value;
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

		public ISchema ValueSchema
		{
			get
			{
				return this.valueSchema;
			}
			private set
			{
				this.valueSchema = value;
			}
		}

		#endregion

		#region Methods/Operators

		private static void AssertCanSet(string propertyName, object propertyValue, object newValue)
		{
			if ((object)propertyValue != null && propertyValue != newValue)
				throw new InvalidOperationException(string.Format("SchemaBuilder: Property '{0}' has already been set.", propertyName));
		}

		public static SchemaBuilder GetArray(ISchema valueSchema)
		{
			SchemaBuilder schemaBuilder;

			if ((object)valueSchema == null)
				throw new ArgumentNullException(nameof(valueSchema));

			schemaBuilder = new SchemaBuilder(SchemaType.Array);
			schemaBuilder.ValueSchema = valueSchema;
			return schemaBuilder;
		}

		public static SchemaBuilder GetMap(ISchema keySchema, ISchema valueSchema)
		{
			SchemaBuilder schemaBuilder;

			if ((object)keySchema == null)
				throw new ArgumentNullException(nameof(keySchema));

			if ((object)valueSchema == null)
				throw new ArgumentNullException(nameof(valueSchema));

			schemaBuilder = new SchemaBuilder(SchemaType.Map);
			schemaBuilder.KeySchema = keySchema;
			schemaBuilder.ValueSchema = valueSchema;
			return schemaBuilder;
		}

		public static SchemaBuilder GetStruct()
		{
			SchemaBuilder schemaBuilder;

			schemaBuilder = new SchemaBuilder(SchemaType.Struct);
			return schemaBuilder;
		}

		public static SchemaBuilder OfSchemaType(SchemaType schemaType)
		{
			return new SchemaBuilder(schemaType);
		}

		public SchemaBuilder AddField(string fieldName, ISchema fieldSchema)
		{
			if (this.SchemaType != SchemaType.Struct)
				throw new InvalidOperationException(string.Format("SchemaBuilder: not a struct type."));

			this.MutableFields.Add(new Field(this.fields.Count, fieldName, fieldSchema));
			return this;
		}

		public SchemaBuilder AddParameter(string name, string value)
		{
			this.MutableParameters.Add(name, value);
			return this;
		}

		public SchemaBuilder AddParameters(IDictionary<string, string> value)
		{
			foreach (KeyValuePair<string, string> item in value)
				this.MutableParameters.Add(item.Key, item.Value);

			return this;
		}

		public SchemaBuilder AsAnonymous()
		{
			bool value = false;
			AssertCanSet(nameof(this.IsIdentity), this.IsIdentity, value);
			this.IsIdentity = value;
			return this;
		}

		public SchemaBuilder AsIdentity()
		{
			bool value = true;
			AssertCanSet(nameof(this.IsIdentity), this.IsIdentity, value);
			this.IsIdentity = value;
			return this;
		}

		public SchemaBuilder AsOptional()
		{
			bool value = true;
			AssertCanSet(nameof(this.IsOptional), this.IsOptional, value);
			this.IsOptional = value;
			return this;
		}

		public SchemaBuilder AsRequired()
		{
			bool value = false;
			AssertCanSet(nameof(this.IsOptional), this.IsOptional, value);
			this.IsOptional = value;
			return this;
		}

		public ISchema Build()
		{
			return this;
		}

		public IField GetFieldByName(string fieldName)
		{
			return null;
		}

		public SchemaBuilder WithDefaultValue(object value)
		{
			AssertCanSet(nameof(this.DefaultValue), this.DefaultValue, value);
			this.DefaultValue = value;
			return this;
		}

		public SchemaBuilder WithDocumentation(string value)
		{
			AssertCanSet(nameof(this.Documentation), this.Documentation, value);
			this.Documentation = value;
			return this;
		}

		public SchemaBuilder WithSchemaName(string value)
		{
			AssertCanSet(nameof(this.SchemaName), this.SchemaName, value);
			this.SchemaName = value;
			return this;
		}

		public SchemaBuilder WithSchemaVersion(int value)
		{
			AssertCanSet(nameof(this.SchemaVersion), this.SchemaVersion, value);
			this.SchemaVersion = value;
			return this;
		}

		#endregion

		/*public static void AssertValidValue(ISchema schema, object value)
		{
			AssertValidValue(null, schema, value);
		}

		public static void AssertValidValue(string name, ISchema schema, object value)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)value == null)
			{
				if (!schema.IsOptional)
					throw new InvalidOperationException(string.Format("SchemaBuilder: Property '{0}' is a required field for schema type '{1}'.", name, schema.SchemaType));
				else
					return;
			}

			List<Class> expectedClasses = LOGICAL_TYPE_CLASSES.get(schema.name());

			if (expectedClasses == null)
				expectedClasses = SCHEMA_TYPE_CLASSES.get(schema.type());

			if (expectedClasses == null)
				throw new DataException("Invalid Java object for schema type " + schema.type()
						+ ": " + value.getClass()
						+ " for field: \"" + name + "\"");

			boolean foundMatch = false;
			for (Class <?> expectedClass : expectedClasses)
			{
				if (expectedClass.isInstance(value))
				{
					foundMatch = true;
					break;
				}
			}
			if (!foundMatch)
				throw new DataException("Invalid Java object for schema type " + schema.type()
						+ ": " + value.getClass()
						+ " for field: \"" + name + "\"");

			switch (schema.type())
			{
				case STRUCT:
					Struct struct = (Struct) value;
                if (!struct.schema().equals(schema))
                    throw new DataException("Struct schemas do not match.");
		struct.validate();
                break;
            case ARRAY:
                List<?> array = (List <?>) value;
                for (Object entry : array)

					validateValue(schema.valueSchema(), entry);
                break;
            case MAP:
                Map<?, ?> map = (Map <?, ?>) value;
                for (Map.Entry<?, ?> entry : map.entrySet()) {

					validateValue(schema.keySchema(), entry.getKey());

					validateValue(schema.valueSchema(), entry.getValue());
	}
                break;
        }
    }*/
	}
}