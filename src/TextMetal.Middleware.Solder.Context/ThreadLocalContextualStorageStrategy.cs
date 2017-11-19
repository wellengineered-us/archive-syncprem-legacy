/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace TextMetal.Middleware.Solder.Context
{
	public sealed class ThreadLocalContextualStorageStrategy : IContextualStorageStrategy
	{
		#region Constructors/Destructors

		public ThreadLocalContextualStorageStrategy()
			: this(() => new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase))
		{
		}

		public ThreadLocalContextualStorageStrategy(Func<IDictionary<string, object>> tlsContextFactoryMethod)
		{
			if ((object)tlsContextFactoryMethod == null)
				throw new ArgumentNullException(nameof(tlsContextFactoryMethod));

			this.threadLocal = new ThreadLocal<IDictionary<string, object>>(tlsContextFactoryMethod);
		}

		#endregion

		#region Fields/Constants

		private readonly ThreadLocal<IDictionary<string, object>> threadLocal;

		#endregion

		#region Properties/Indexers/Events

		private ThreadLocal<IDictionary<string, object>> ThreadLocal
		{
			get
			{
				return this.threadLocal;
			}
		}

		#endregion

		#region Methods/Operators

		public T GetValue<T>(string key)
		{
			object value;
			this.ThreadLocal.Value.TryGetValue(key, out value);
			return (T)value;
		}

		public bool HasValue(string key)
		{
			return this.ThreadLocal.Value.ContainsKey(key);
		}

		public void RemoveValue(string key)
		{
			if (this.ThreadLocal.Value.ContainsKey(key))
				this.ThreadLocal.Value.Remove(key);
		}

		public void SetValue<T>(string key, T value)
		{
			if (this.ThreadLocal.Value.ContainsKey(key))
				this.ThreadLocal.Value.Remove(key);

			this.ThreadLocal.Value.Add(key, value);
		}

		#endregion
	}
}