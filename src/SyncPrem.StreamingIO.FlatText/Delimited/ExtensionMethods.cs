/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.FlatText.Delimited
{
	public static class ExtensionMethods
	{
		#region Methods/Operators

		public static IEnumerable<IDelimitedTextFieldSpec> ToDelimitedTextFieldSpecs(this IEnumerable<IField> fields)
		{
			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			return fields.Select(f => new DelimitedTextFieldSpec()
									{
										Schema = null,
										FieldType = f.FieldType,
										FieldIndex = f.FieldIndex,
										FieldName = f.FieldName,
										IsKeyComponent = f.IsKeyComponent,
										IsOptional = f.IsOptional
									});
		}

		public static IEnumerable<IField> ToFields(this IEnumerable<IDelimitedTextFieldSpec> specs)
		{
			if ((object)specs == null)
				throw new ArgumentNullException(nameof(specs));

			return specs.Select(f => new Field()
									{
										Schema = null,
										FieldType = f.FieldType,
										FieldIndex = f.FieldIndex,
										FieldName = f.FieldName,
										IsKeyComponent = f.IsKeyComponent,
										IsOptional = f.IsOptional
									});
		}

		#endregion
	}
}