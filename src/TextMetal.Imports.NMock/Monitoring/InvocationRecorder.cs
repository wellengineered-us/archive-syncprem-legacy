using System;

namespace NMock.Monitoring
{
	internal class InvocationRecorder : IDisposable
	{
		[ThreadStatic]
		private static InvocationRecorder _instance;

		internal InvocationRecorder()
		{
			_instance = this;
		}

		internal static InvocationRecorder Current
		{
			get
			{
				return _instance;
			}
		}
		internal static bool Recording
		{
			get
			{
				return _instance != null;
			}
		}

		public Invocation Invocation { get; set; }

		public void Dispose()
		{
			_instance = null;
		}
	}
}
