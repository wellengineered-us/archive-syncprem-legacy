/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TextMetal.Middleware.Solder.Primitives
{
	public static class LeakDetector
	{
		#region Fields/Constants

		private static readonly ConcurrentDictionary<Guid, DisposableList<IDisposable>> trackedResources = new ConcurrentDictionary<Guid, DisposableList<IDisposable>>();

		#endregion

		#region Properties/Indexers/Events

		private static ConcurrentDictionary<Guid, DisposableList<IDisposable>> TrackedResources
		{
			get
			{
				return trackedResources;
			}
		}

		#endregion

		#region Methods/Operators

		[Conditional("DEBUG")]
		public static void __check(this object obj, [CallerMemberName] string value = null)
		{
			StringBuilder sb;

			int count = 0;
			sb = new StringBuilder();

			foreach (KeyValuePair<Guid, DisposableList<IDisposable>> trackedResource in TrackedResources)
			{
				using (trackedResource.Value)
				{
					sb.AppendFormat("[{0} -> {1}]", trackedResource.Key, trackedResource.Value.Count);
					sb.AppendLine();
				}

				if(trackedResource.Value.Count > 0)
					count++;
			}

			__print(obj, string.Format("leak_check {0}", sb.ToString()), value);

			if (count > 0)
			{
				__print(obj, string.Format("leak_check FAILED with {0} leaked disposables", count), value);
				Environment.FailFast(string.Format("leak_check FAILED with {0} leaked disposables", count));
			}
		}

		[Conditional("DEBUG")]
		public static void __disp<T>(this object obj, T disposable, [CallerMemberName] string value = null)
			where T : IDisposable
		{
			__disp(obj, Guid.Empty, disposable, value);
		}

		[Conditional("DEBUG")]
		public static void __disp<T>(this object obj, Guid _, T disposable, [CallerMemberName] string value = null)
			where T : IDisposable
		{
			DisposableList<IDisposable> disps;

			if (!TrackedResources.TryGetValue(_, out disps) || !disps.Contains(disposable))
				throw new InvalidOperationException();

			disps.Remove(disposable);

			__print(obj, string.Format("dispose ({1})@{0:N}", _, disposable.GetType().Name), value);
		}

		public static Guid __enter(this object obj, [CallerMemberName] string value = null)
		{
#if DEBUG
			Guid _ = Guid.NewGuid();

			__print(obj, string.Format("enter@{0:N}", _), value);

			return _;
#else
			return Guid.Empty;
#endif
		}

		[Conditional("DEBUG")]
		public static void __leave(this object obj, Guid _, [CallerMemberName] string value = null)
		{
			__print(obj, string.Format("leave@{0:N}", _), value);
		}

		[Conditional("DEBUG")]
		public static void __leave(this object obj, [CallerMemberName] string value = null)
		{
			__leave(obj, Guid.Empty, value);
		}

		[Conditional("DEBUG")]
		private static void __print(object obj, string message, string value)
		{
			/* THIS METHOD SHOULD NOT BE DEFINED IN RELEASE/PRODUCTION BUILDS */
			string temp = string.Format("{0}::{1}: {2}", obj?.GetType().Name, value, message);

			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(temp);
			Console.ForegroundColor = oldConsoleColor;
		}

		[Conditional("DEBUG")]
		public static void __trace(this object obj, string message, [CallerMemberName] string value = null)
		{
			__trace(obj, Guid.Empty, message, value);
		}

		[Conditional("DEBUG")]
		public static void __trace(this object obj, Guid _, string message, [CallerMemberName] string value = null)
		{
			__print(obj, string.Format("{1}@{0:N}", _, message), value);
		}

		public static T __use<T>(this object obj, T disposable, [CallerMemberName] string value = null)
			where T : IDisposable
		{
			return __use(obj, Guid.Empty, disposable, value);
		}

		public static T __use<T>(this object obj, Guid _, T disposable, [CallerMemberName] string value = null)
			where T : IDisposable
		{
#if DEBUG
			DisposableList<IDisposable> disps;

			if (TrackedResources.TryGetValue(_, out disps))
			{
				if (disps.Contains(disposable))
					throw new InvalidOperationException();
			}
			else
			{
				disps = new DisposableList<IDisposable>();
				TrackedResources.TryAdd(_, disps);
			}

			disps.Add(disposable);

			__print(obj, string.Format("using ({1})@{0:N}", _, disposable.GetType().Name), value);
#endif
			return disposable;
		}

		#endregion
	}
}