/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Primitives
{
	public sealed class Partition : Dictionary<string, object>, IPartition
	{
		#region Constructors/Destructors

		private Partition()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly Partition none = new Partition();

		#endregion

		#region Properties/Indexers/Events

		public static Partition None
		{
			get
			{
				return none;
			}
		}

		#endregion
	}
}