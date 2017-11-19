/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace TextMetal.Middleware.Solder.Primitives
{
	public class DisposableList<TDisposable> : List<TDisposable>, IDisposable
		where TDisposable : IDisposable
	{
		#region Constructors/Destructors

		public DisposableList()
		{
		}

		public DisposableList(IEnumerable<TDisposable> disposables)
		{
			if ((object)disposables == null)
				throw new ArgumentNullException(nameof(disposables));

			this.AddRange(disposables);
		}

		#endregion

		#region Methods/Operators

		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (TDisposable disposable in this)
				{
					if ((object)disposable != null)
						disposable.Dispose();
				}
			}
		}

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}