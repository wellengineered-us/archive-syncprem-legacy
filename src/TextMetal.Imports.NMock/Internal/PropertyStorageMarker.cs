using System;
using NMock.Monitoring;

namespace NMock.Internal
{
	internal class PropertyStorageMarker : IDisposable
	{
		[ThreadStatic]
		private static PropertyStorageMarker _instance;

		internal PropertyStorageMarker()
		{
			_instance = this;
		}

		internal static bool UsePropertyStorage
		{
			get
			{
				return _instance != null;
			}
		}

		public void Dispose()
		{
			_instance = null;
		}

	}
}
