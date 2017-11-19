/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Extensions
{
	public static class UtilitiesExtensionMethods
	{
		#region Methods/Operators

		public static T ChangeType<T>(this object value)
		{
			return SolderFascadeAccessor.DataTypeFascade.ChangeType<T>(value);
		}

		public static object ChangeType(this object value, Type conversionType)
		{
			return SolderFascadeAccessor.DataTypeFascade.ChangeType(value, conversionType);
		}

		public static string SafeToString<TValue>(this TValue value)
		{
			return SolderFascadeAccessor.DataTypeFascade.SafeToString<TValue>(value);
		}

		public static string SafeToString<TValue>(this TValue value, string format)
		{
			return SolderFascadeAccessor.DataTypeFascade.SafeToString<TValue>(value, format);
		}

		public static string SafeToString<TValue>(this TValue value, string format, string @default)
		{
			return SolderFascadeAccessor.DataTypeFascade.SafeToString<TValue>(value, format, @default);
		}

		public static string SafeToString<TValue>(this TValue value, string format, string @default, bool dofvisnow)
		{
			return SolderFascadeAccessor.DataTypeFascade.SafeToString<TValue>(value, format, @default, dofvisnow);
		}

		#endregion
	}
}