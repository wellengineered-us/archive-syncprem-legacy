using System.IO;
using NMock.Internal;
using NMock.Monitoring;

namespace NMock.Actions
{
	internal class ReturnPropertyValueAction : IAction
	{
		private readonly IMockObject _mockObject;

		public ReturnPropertyValueAction(IMockObject mockObject)
		{
			_mockObject = mockObject;
		}

		void IAction.Invoke(Invocation invocation)
		{
			if(invocation.IsPropertyGetAccessor)
			{
				using(new PropertyStorageMarker())
				{
					invocation.InvokeOn(_mockObject);
				}
			}
		}

		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("return the set value");
		}
	}
}