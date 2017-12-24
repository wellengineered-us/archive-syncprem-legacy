/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Primitives.Schema
{
	public enum SchemaType
	{
		Integer08,
		Integer16,
		Integer32,
		Integer64,

		UnsignedInteger08,
		UnsignedInteger16,
		UnsignedInteger32,
		UnsignedInteger64,

		Float32,
		Float64,
		Decimal,

		Character,
		Boolean,
		DateTime,
		TimeSpan,
		Guid,

		String,
		Bytes,

		Array,
		Map,
		Struct,
		Void
	}
}