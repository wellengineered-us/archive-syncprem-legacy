/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.Solder.Interception;

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	public class MockRuntimeInterception : RuntimeInterception
	{
		#region Constructors/Destructors

		public MockRuntimeInterception()
		{
		}

		#endregion

		#region Fields/Constants

		private string lastOperationName;

		#endregion

		#region Properties/Indexers/Events

		public string LastOperationName
		{
			get
			{
				return this.lastOperationName;
			}
			private set
			{
				this.lastOperationName = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void OnInvoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
		{
			this.LastOperationName = string.Format("{0}::{1}", (object)runtimeInvocation.TargetType == null ? "<null>" : runtimeInvocation.TargetType.Name, (object)runtimeInvocation.TargetMethod == null ? "<null>" : runtimeInvocation.TargetMethod.Name);

			if ((object)runtimeInvocation.TargetMethod != null)
			{
				if (runtimeInvocation.TargetMethod.DeclaringType == typeof(object) ||
					runtimeInvocation.TargetMethod.DeclaringType == typeof(IDisposable) ||
					runtimeInvocation.TargetMethod.DeclaringType == typeof(IMockCloneable))
					runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(this, runtimeInvocation.InvocationArguments);
			}

			throw new InvalidOperationException(string.Format("Method '{0}' not supported on '{1}'.", (object)runtimeInvocation.TargetMethod == null ? "<null>" : runtimeInvocation.TargetMethod.Name, runtimeInvocation.TargetType.FullName));
		}

		#endregion
	}
}