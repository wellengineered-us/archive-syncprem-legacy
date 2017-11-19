/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Interception
{
	internal sealed class RuntimeContext : IRuntimeContext
	{
		#region Constructors/Destructors

		public RuntimeContext()
		{
		}

		#endregion

		#region Fields/Constants

		private bool continueInterception;
		private int interceptionCount;
		private int interceptionIndex;

		#endregion

		#region Properties/Indexers/Events

		public bool ContinueInterception
		{
			get
			{
				return this.continueInterception;
			}
			internal set
			{
				this.continueInterception = value;
			}
		}

		public int InterceptionCount
		{
			get
			{
				return this.interceptionCount;
			}
			internal set
			{
				this.interceptionCount = value;
			}
		}

		public int InterceptionIndex
		{
			get
			{
				return this.interceptionIndex;
			}
			internal set
			{
				this.interceptionIndex = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void AbortInterceptionChain()
		{
			if (!this.ContinueInterception)
				throw new InvalidOperationException(string.Format("Cannot abort interception chain; the chain has was either previously aborted or pending graceful completion."));

			this.continueInterception = false;
		}

		#endregion
	}
}