using System.IO;
using NMock.Monitoring;

namespace NMock.Actions
{
	internal class CaptureValueAction : IAction
	{
		void IAction.Invoke(Invocation invocation)
		{
			invocation.SetterResult = invocation.Arguments[invocation.Arguments.Length - 1];
		}

		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("capture the setter value");
		}
	}
}