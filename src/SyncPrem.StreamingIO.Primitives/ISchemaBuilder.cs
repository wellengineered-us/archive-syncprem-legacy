/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives
{
	public interface ISchemaBuilder
	{
		#region Methods/Operators

		SchemaBuilder AddField(string fieldName, Type fieldType, bool isFieldOptional, bool isFieldKeyPart, ISchema fieldSchema = null);

		SchemaBuilder AddFields(IEnumerable<IField> fields);

		ISchema Build();

		SchemaBuilder WithName(string value);

		SchemaBuilder WithType(SchemaType value);

		SchemaBuilder WithVersion(int value);

		#endregion
	}
}