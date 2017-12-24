/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public static class __
	{
		#region Methods/Operators

		public static void main()
		{
			ISchema int08 = SchemaBuilder.OfSchemaType(SchemaType.Integer08).Build();
			ISchema int16 = SchemaBuilder.OfSchemaType(SchemaType.Integer16).Build();
			ISchema int32 = SchemaBuilder.OfSchemaType(SchemaType.Integer32).Build();
			ISchema int64 = SchemaBuilder.OfSchemaType(SchemaType.Integer64).Build();

			ISchema stringSchema = SchemaBuilder.OfSchemaType(SchemaType.String).Build();
			ISchema voidSchema = SchemaBuilder.OfSchemaType(SchemaType.Void).Build();

			ISchema schema = SchemaBuilder.GetStruct()
				.AddField("Bob", int08)
				.AddField("Jim", int16)
				.Build();

			ISchema map = SchemaBuilder.GetArray(SchemaBuilder.GetMap(stringSchema, voidSchema)).Build();
		}

		#endregion
	}

	public interface ISchema
	{
		#region Properties/Indexers/Events

		object DefaultValue
		{
			get;
		}

		string Documentation
		{
			get;
		}

		bool IsIdentity
		{
			get;
		}

		bool IsOptional
		{
			get;
		}

		bool IsPrimitiveType
		{
			get;
		}

		IReadOnlyDictionary<string, string> Parameters
		{
			get;
		}

		ISchema Schema
		{
			get;
		}

		string SchemaName
		{
			get;
		}

		SchemaType SchemaType
		{
			get;
		}

		int SchemaVersion
		{
			get;
		}

		#endregion
	}
}