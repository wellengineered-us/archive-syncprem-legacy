/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.StreamingIO.Primitives
{
	public sealed class SchemaBuilder : ISchema
	{
		#region Constructors/Destructors

		public SchemaBuilder()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Dictionary<string, IField> fields = new Dictionary<string, IField>(StringComparer.OrdinalIgnoreCase);
		private string schemaName;
		private int schemaVersion;

		#endregion

		#region Properties/Indexers/Events

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
			if ((object)propertyValue != null && propertyValue != newValue)
				throw new InvalidOperationException(string.Format("SchemaBuilder: Property '{0}' has already been set.", propertyName));
		}

		public static SchemaBuilder Create()
		{
			return new SchemaBuilder();
		}

		public SchemaBuilder AddField(string fieldName, Type type = null, bool key = false, bool optional = true, ISchema schema = null)
		{
			if ((object)fieldName == null)
				throw new ArgumentNullException(nameof(fieldName));

			this.MutableFields.Add(fieldName, new Field()
											{
												FieldIndex = this.MutableFields.Count,
												FieldName = fieldName,
												FieldType = type ?? typeof(String),
												IsKeyComponent = key,
												IsOptional = optional,
												Schema = schema
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
			return string.Join(", ", this.Fields.Values.Select(f => $"[{f.FieldName}] AS [{f.FieldType.Name}]"));
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

		#endregion
	}
}