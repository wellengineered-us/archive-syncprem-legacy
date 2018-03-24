/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.StreamingIO.Primitives
{
	public sealed class Payload : Dictionary<string, object>, IPayload
	{
		#region Constructors/Destructors

		public Payload()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion

		#region Methods/Operators

		public static Payload FromPrimitive<T>(T value)
		{
			return FromPrimitive((object)value);
		}

		public static Payload FromPrimitive(object value)
		{
			Payload payload;

			payload = new Payload();
			payload.Add(string.Empty, value);

			return payload;
		}

		public override string ToString()
		{
			return string.Join(", ", this.Select(kv => $"{kv.Key}={kv.Value}"));
		}

		#endregion
	}
}