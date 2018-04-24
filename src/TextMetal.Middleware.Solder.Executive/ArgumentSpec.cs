/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Executive
{
	public class ArgumentSpec
	{
		#region Constructors/Destructors

		public ArgumentSpec(Type type, bool required, bool bounded)
		{
			this.type = type ?? typeof(Object);
			this.required = required;
			this.bounded = bounded;
		}

		#endregion

		#region Fields/Constants

		private readonly bool bounded;
		private readonly bool required;
		private readonly Type type;

		#endregion

		#region Properties/Indexers/Events

		public bool Bounded
		{
			get
			{
				return this.bounded;
			}
		}

		public bool Required
		{
			get
			{
				return this.required;
			}
		}

		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		#endregion
	}
}