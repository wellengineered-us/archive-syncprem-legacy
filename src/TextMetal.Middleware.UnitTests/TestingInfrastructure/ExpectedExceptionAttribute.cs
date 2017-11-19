using System;

using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ExpectedExceptionAttribute : NUnitAttribute, IWrapTestMethod
	{
		#region Constructors/Destructors

		public ExpectedExceptionAttribute(Type type)
		{
			this.expectedExceptionType = type;
		}

		#endregion

		#region Fields/Constants

		private readonly Type expectedExceptionType;

		#endregion

		#region Methods/Operators

		public TestCommand Wrap(TestCommand command)
		{
			return new ExpectedExceptionCommand(command, this.expectedExceptionType);
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		private class ExpectedExceptionCommand : DelegatingTestCommand
		{
			#region Constructors/Destructors

			public ExpectedExceptionCommand(TestCommand innerCommand, Type expectedType)
				: base(innerCommand)
			{
				this._expectedType = expectedType;
			}

			#endregion

			#region Fields/Constants

			private readonly Type _expectedType;

			#endregion

			#region Methods/Operators

			public override TestResult Execute(TestExecutionContext context)
			{
				Type caughtType = null;

				try
				{
					this.innerCommand.Execute(context);
				}
				catch (Exception ex)
				{
					if (ex is NUnitException)
						ex = ex.InnerException;
					caughtType = ex.GetType();
				}

				if (caughtType == this._expectedType)
					context.CurrentResult.SetResult(ResultState.Success);
				else if (caughtType != null)
				{
					context.CurrentResult.SetResult(ResultState.Failure,
						string.Format("Expected {0} but got {1}", this._expectedType.Name, caughtType.Name));
				}
				else
				{
					context.CurrentResult.SetResult(ResultState.Failure,
						string.Format("Expected {0} but no exception was thrown", this._expectedType.Name));
				}

				return context.CurrentResult;
			}

			#endregion
		}

		#endregion
	}
}