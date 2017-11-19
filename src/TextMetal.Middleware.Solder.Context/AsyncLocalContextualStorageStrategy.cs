/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace TextMetal.Middleware.Solder.Context
{
	public sealed class AsyncLocalContextualStorageStrategy : IContextualStorageStrategy
	{
		#region Constructors/Destructors

		public AsyncLocalContextualStorageStrategy()
			: this(new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase))
		{
		}

		public AsyncLocalContextualStorageStrategy(IDictionary<string, object> asyncContext)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			this.asyncLocal = new AsyncLocal<IDictionary<string, object>>() { Value = asyncContext };
		}

		#endregion

		#region Fields/Constants

		private readonly AsyncLocal<IDictionary<string, object>> asyncLocal;

		#endregion

		#region Properties/Indexers/Events

		private AsyncLocal<IDictionary<string, object>> AsyncLocal
		{
			get
			{
				return this.asyncLocal;
			}
		}

		#endregion

		#region Methods/Operators

		public T GetValue<T>(string key)
		{
			object value;
			this.AsyncLocal.Value.TryGetValue(key, out value);
			return (T)value;
		}

		public bool HasValue(string key)
		{
			return this.AsyncLocal.Value.ContainsKey(key);
		}

		public void RemoveValue(string key)
		{
			if (this.AsyncLocal.Value.ContainsKey(key))
				this.AsyncLocal.Value.Remove(key);
		}

		public void SetValue<T>(string key, T value)
		{
			if (this.AsyncLocal.Value.ContainsKey(key))
				this.AsyncLocal.Value.Remove(key);

			this.AsyncLocal.Value.Add(key, value);
		}

		#endregion
	}
}