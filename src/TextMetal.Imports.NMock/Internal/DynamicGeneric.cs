#if NETFX40
// Copyright:     (C) 2011 by Zachary Gramana
// Author:        Zachary Gramana
// Version:       0.1
// Description:   Wraps any class inside of a DynamicObject.
// StartedOn:     7/20/2011
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace NMock.Internal
{
	/// <summary>
	/// Wraps any object in a DynamicObject, but in a strongly-typed way.
	/// It also logs usage of the object to standard output.
	/// 
	/// Running:
	///     dynamic item = new Dynamic&lt;String&gt;("Jabberwocky");
	///     item = new Dynamic&lt;AppDomain&gt;(AppDomain.CurrentDomain);
	///     item.SetData("Foo", "Bar");
	///     item.GetData("Foo");
	///     item.Foo = "Bar";
	///     var bar = item.Foo;
	/// 
	/// Outputs:
	///     Dynamic's Inherited Properties:
	///             Chars
	///             Length
	/// 
	///     Dynamic's Inherited Properties:
	///             DomainManager
	///             CurrentDomain
	///             Evidence
	///             FriendlyName
	///             BaseDirectory
	///             RelativeSearchPath
	///             ShadowCopyFiles
	///             ActivationContext
	///             ApplicationIdentity
	///             ApplicationTrust
	///             DynamicDirectory
	///             SetupInformation
	///             PermissionSet
	///             IsFullyTrusted
	///             IsHomogenous
	///             Id
	///             MonitoringIsEnabled
	///             MonitoringTotalProcessorTime
	///             MonitoringTotalAllocatedMemorySize
	///             MonitoringSurvivedMemorySize
	///             MonitoringSurvivedProcessMemorySize
	/// 
	/// Results:
	///     Invoking instance.SetData() returned '(null)'
	/// 
	/// Results:
	///     Invoking instance.GetData() returned 'Bar'
	/// 
	/// Results:
	///     Setting instance.Foo = Bar
	/// 
	/// Results:
	///     Getting instance.Foo returned 'Bar'
	/// 
	/// </summary>
	internal class Dynamic<T> : DynamicObject
	{
		private static readonly Dictionary<string, object> _propertyCache;
		private readonly T _instance;
		private readonly Dictionary<string, object> _propertyBag;

		/// <summary>
		/// Our type constructor. This populates our lookup store with
		/// all of the property information we need about the object
		/// we are wrapping. This runs just once, the first time
		/// we make an instance of the a particular type.
		/// </summary>
		static Dynamic()
		{
			_propertyCache = typeof(T)
				.GetProperties()
				.ToDictionary(
					k => k.Name,
					k => (Object)k
				   );

			Console.WriteLine("\r\nDynamic's Inherited Properties:", typeof(T).Name);
			foreach (var prop in _propertyCache)
			{
				Console.WriteLine("\t" + prop.Key);
			}
		}

		/// <summary>
		/// Our instance constructor.
		/// </summary>
		/// The object we are wrapping.
		public Dynamic(T instance)
		{
			_instance = instance;
			_propertyBag = new Dictionary<string, object>(StringComparer.CurrentCulture);

			// Log this action.
			Console.WriteLine("\r\nCreated new {0} wrapper object.", this.GetType().Name.TrimEnd(new[] { '`', '1' }), this.GetType().GetGenericArguments()[0].Name);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			Boolean value;

			Object prop;
			value = _propertyCache.TryGetValue(binder.Name, out prop);

			if (value)
			{
				result = ((PropertyInfo)prop).GetValue(_instance, null);
				value = true;
			}
			else
			{
				value = _propertyBag.TryGetValue(binder.Name, out result);
			}

			// Log this action.
			Console.WriteLine("\r\nResults:\r\n\tGetting instance.{0} returned '{1}'", binder.Name, result ?? "(null)");

			return value;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			Object prop;
			var foundProp = _propertyCache.TryGetValue(binder.Name, out prop);

			if (foundProp)
			{
				((PropertyInfo)prop).SetValue(_instance, value, null);
			}
			else
			{
				_propertyBag[binder.Name] = value;
			}

			// Log this action.
			Console.WriteLine("\r\nResults:\r\n\tSetting instance.{0} = {1}", binder.Name, value ?? "(null)");

			// You can always add a value to a dictionary,
			// so this method always returns true.
			return true;
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			// Use reflection to get the method info.
			var method = typeof(T)
				.GetMethod(
					binder.Name,
					args.Select(a => a.GetType())
						.ToArray()
			   );

			if (method == null)
			{
				result = null;
				return false;
			}

			result = method.Invoke(_instance, args);

			// Log this action.
			Console.WriteLine("\r\nResults:\r\n\tInvoking instance.{0}() returned '{1}'", method.Name, result ?? "(null)");

			// If we didn't throw an exception, we succeeded.
			return true;
		}

		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
		{
			return base.TryInvoke(binder, args, out result);
		}

		public T GetInstance()
		{
			return _instance;
		}
	}
}

#endif