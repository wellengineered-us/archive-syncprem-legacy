/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace TextMetal.Middleware.Solder.Context
{
	public sealed class GlobalStaticContextualStorageStrategy : IContextualStorageStrategy
	{
		#region Constructors/Destructors

		public GlobalStaticContextualStorageStrategy()
			: this(LazySingleton.__)
		{
		}

		public GlobalStaticContextualStorageStrategy(IDictionary<string, object> sharedStatic)
		{
			if ((object)sharedStatic == null)
				throw new ArgumentNullException(nameof(sharedStatic));

			this.sharedStatic = sharedStatic;
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<string, object> sharedStatic;

		#endregion

		#region Properties/Indexers/Events

		private IDictionary<string, object> SharedStatic
		{
			get
			{
				return this.sharedStatic;
			}
		}

		#endregion

		#region Methods/Operators

		public T GetValue<T>(string key)
		{
			object value;
			this.SharedStatic.TryGetValue(key, out value);
			return (T)value;
		}

		public bool HasValue(string key)
		{
			return this.SharedStatic.ContainsKey(key);
		}

		public void RemoveValue(string key)
		{
			if (this.SharedStatic.ContainsKey(key))
				this.SharedStatic.Remove(key);
		}

		public void ResetValues()
		{
			this.SharedStatic.Clear();
		}

		public void SetValue<T>(string key, T value)
		{
			if (this.SharedStatic.ContainsKey(key))
				this.SharedStatic.Remove(key);

			this.SharedStatic.Add(key, value);
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		/// <summary>
		/// http://www.yoda.arachsys.com/csharp/singleton.html
		/// </summary>
		private class LazySingleton
		{
			#region Constructors/Destructors

			/// <summary>
			/// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
			/// </summary>
			static LazySingleton()
			{
			}

			#endregion

			#region Fields/Constants

			internal static readonly IDictionary<string, object> __ = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

			#endregion
		}

		#endregion
	}
}