/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;

namespace SyncPrem.StreamingIO.Primitives
{
	public class KeyValueView
	{
		#region Constructors/Destructors

		public KeyValueView(ISchema schema, IPayload payload)
		{
			IPayload key, value;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)payload == null)
				throw new ArgumentNullException(nameof(payload));

			var groups = schema.Fields.Values.OrderBy(f => f.FieldIndex).GroupBy(f => f.IsFieldKeyComponent);

			key = new Payload();
			value = new Payload();

			foreach (IGrouping<bool, IField> grouping in groups)
			{
				foreach (IField field in grouping)
				{
					(grouping.Key ? key : value).Add(field.FieldName, payload[field.FieldName]);
				}
			}

			this.schema = schema;
			this.payload = payload;
			this.key = key;
			this.value = value;
		}

		#endregion

		#region Fields/Constants

		private readonly IPayload key;
		private readonly IPayload payload;
		private readonly ISchema schema;
		private readonly IPayload value;

		#endregion

		#region Properties/Indexers/Events

		public IPayload Key
		{
			get
			{
				return this.key;
			}
		}

		public IPayload Payload
		{
			get
			{
				return this.payload;
			}
		}

		public ISchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		public IPayload Value
		{
			get
			{
				return this.value;
			}
		}

		#endregion
	}
}