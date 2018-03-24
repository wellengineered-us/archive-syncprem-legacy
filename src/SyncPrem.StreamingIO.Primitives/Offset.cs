/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives
{
	public sealed class Offset : Dictionary<string, object>, IOffset
	{
		#region Constructors/Destructors

		private Offset()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly Offset none = new Offset();

		#endregion

		#region Properties/Indexers/Events

		public static Offset None
		{
			get
			{
				return none;
			}
		}

		#endregion
	}
}