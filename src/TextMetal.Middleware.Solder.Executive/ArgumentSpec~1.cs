/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace TextMetal.Middleware.Solder.Executive
{
	public class ArgumentSpec<T> : ArgumentSpec
	{
		#region Constructors/Destructors

		public ArgumentSpec(bool required, bool bounded)
			: base(typeof(T), required, bounded)
		{
		}

		#endregion
	}
}