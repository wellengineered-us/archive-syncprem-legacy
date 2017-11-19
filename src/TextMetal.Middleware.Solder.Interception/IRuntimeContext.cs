/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace TextMetal.Middleware.Solder.Interception
{
	public interface IRuntimeContext
	{
		#region Properties/Indexers/Events

		bool ContinueInterception
		{
			get;
		}

		int InterceptionCount
		{
			get;
		}

		int InterceptionIndex
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void AbortInterceptionChain();

		#endregion
	}
}