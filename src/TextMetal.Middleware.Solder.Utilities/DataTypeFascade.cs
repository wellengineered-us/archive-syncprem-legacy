/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data;
using System.Globalization;
using System.Xml;

namespace TextMetal.Middleware.Solder.Utilities
{
	/// <summary>
	/// Provides static helper and/or extension methods for core data type functionality such as validation and parsing.
	/// </summary>
	public sealed class DataTypeFascade : IDataTypeFascade
	{
		#region Constructors/Destructors

		public DataTypeFascade()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Performs a run-time type change on a given value.
		/// </summary>
		/// <typeparam name="T"> The type to change value to. </typeparam>
		/// <param name="value"> The value to change type. </param>
		/// <returns> A value changed to the given type. </returns>
		public T ChangeType<T>(object value)
		{
			return (T)this.ChangeType(value, typeof(T));
		}

		/// <summary>
		/// Performs a run-time type change on a given value.
		/// </summary>
		/// <param name="value"> The value to change type. </param>
		/// <param name="conversionType"> The type to change value to. </param>
		/// <returns> A value changed to the given type. </returns>
		public object ChangeType(object value, Type conversionType)
		{
			if ((object)conversionType == null)
				throw new ArgumentNullException(nameof(conversionType));

			if ((object)value == null || value == DBNull.Value)
				return this.DefaultValue(conversionType);

			if (conversionType.IsAssignableFrom(value.GetType()))
				return value;

			if (conversionType.IsGenericType &&
				!conversionType.IsGenericTypeDefinition &&
				conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
				conversionType = Nullable.GetUnderlyingType(conversionType);

			return Convert.ChangeType(value, conversionType);
		}

		/// <summary>
		/// Obtains the default value for a given type using reflection.
		/// </summary>
		/// <param name="targetType"> The target type. </param>
		/// <returns> The default value for the target type. </returns>
		public object DefaultValue(Type targetType)
		{
			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
		}

		/// <summary>
		/// Returns a DbType mapping for a Type.
		/// An InvalidOperationException is thrown for unmappable types.
		/// </summary>
		/// <param name="clrType"> The CLR type to map to a DbType. </param>
		/// <returns> The mapped DbType. </returns>
		public DbType InferDbTypeForClrType(Type clrType)
		{
			if ((object)clrType == null)
				throw new ArgumentNullException(nameof(clrType));

			if (clrType.IsByRef /* || type.IsPointer || type.IsArray */)
				return this.InferDbTypeForClrType(clrType.GetElementType());
			else if (clrType.IsGenericType &&
					!clrType.IsGenericTypeDefinition &&
					clrType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return this.InferDbTypeForClrType(Nullable.GetUnderlyingType(clrType));
			else if (clrType.IsEnum)
				return this.InferDbTypeForClrType(Enum.GetUnderlyingType(clrType));
			else if (clrType == typeof(Boolean))
				return DbType.Boolean;
			else if (clrType == typeof(Byte))
				return DbType.Byte;
			else if (clrType == typeof(DateTime))
				return DbType.DateTime;
			else if (clrType == typeof(DateTimeOffset))
				return DbType.DateTimeOffset;
			else if (clrType == typeof(Decimal))
				return DbType.Decimal;
			else if (clrType == typeof(Double))
				return DbType.Double;
			else if (clrType == typeof(Guid))
				return DbType.Guid;
			else if (clrType == typeof(Int16))
				return DbType.Int16;
			else if (clrType == typeof(Int32))
				return DbType.Int32;
			else if (clrType == typeof(Int64))
				return DbType.Int64;
			else if (clrType == typeof(SByte))
				return DbType.SByte;
			else if (clrType == typeof(Single))
				return DbType.Single;
			else if (clrType == typeof(TimeSpan))
				return DbType.Time;
			else if (clrType == typeof(UInt16))
				return DbType.UInt16;
			else if (clrType == typeof(UInt32))
				return DbType.UInt32;
			else if (clrType == typeof(UInt64))
				return DbType.UInt64;
			else if (clrType == typeof(Byte[]))
				return DbType.Binary;
			else if (clrType == typeof(Boolean[]))
				return DbType.Byte;
			else if (clrType == typeof(String))
				return DbType.String;
			else if (clrType == typeof(XmlDocument))
				return DbType.Xml;
			else if (clrType == typeof(Object))
				return DbType.Object;
			else
				throw new InvalidOperationException(string.Format("Cannot infer parameter type from unsupported CLR type '{0}'.", clrType.FullName));
		}

		/// <summary>
		/// Determines if a string value is null or zero length.
		/// </summary>
		/// <param name="value"> The string value to check. </param>
		/// <returns> A boolean value indicating whether the value is null or zero length. </returns>
		public bool IsNullOrEmpty(string value)
		{
			return (object)value == null || value.Length <= 0;
		}

		/// <summary>
		/// Determines if a string value is null, zero length, or only contains white space.
		/// </summary>
		/// <param name="value"> The string value to check. </param>
		/// <returns> A boolean value indicating whether the value is null, zero length, or only contains white space. </returns>
		public bool IsNullOrWhiteSpace(string value)
		{
			return (object)value == null || this.IsWhiteSpace(value);
		}

		/// <summary>
		/// Determines if a string value is zero length or only contains white space.
		/// </summary>
		/// <param name="value"> The string value to check. </param>
		/// <returns> A boolean value indicating whether the value is zero length or only contains white space. </returns>
		public bool IsWhiteSpace(string value)
		{
			if ((object)value == null)
				return false;

			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Checks whether two object instances are equal using the Object.Equals() method. Value coercion is performed.
		/// </summary>
		/// <param name="objA"> An object instance or null. </param>
		/// <param name="objB"> Another object instance or null. </param>
		/// <returns> A value indicating whether the two object instances are equal. </returns>
		public bool ObjectsEqualValueSemantics(object objA, object objB)
		{
			Type typeOfA = null, typeOfB = null;

			if (((object)objA != null && (object)objB == null) ||
				((object)objA == null && (object)objB != null))
				return false; // prevent null coalescence

			if ((object)objA != null)
				typeOfA = objA.GetType();

			if ((object)objB != null)
				typeOfB = objB.GetType();

			return ((object)objA != null ? objA.Equals(this.ChangeType(objB, typeOfA)) : ((object)objB != null ? objB.Equals(this.ChangeType(objA, typeOfB)) : true /* both null */));
		}

		/// <summary>
		/// Returns a string that represents the specified type with the format specification. If the value is null, then the default value of a zero length string is returned.
		/// </summary>
		/// <typeparam name="TValue"> The type of the value to obtain a string representation. </typeparam>
		/// <param name="value"> The target value. </param>
		/// <returns> A formatted string value if the value is not null; otherwise the default value specified. </returns>
		public string SafeToString<TValue>(TValue value)
		{
			return this.SafeToString<TValue>(value, null, string.Empty);
		}

		/// <summary>
		/// Returns a string that represents the specified type with the format specification. If the value is null, then the default value of a zero length string is returned. No trimming is performed.
		/// </summary>
		/// <typeparam name="TValue"> The type of the value to obtain a string representation. </typeparam>
		/// <param name="value"> The target value. </param>
		/// <param name="format"> The string specifying the format to use or null to use the default format defined for the type of the IFormattable implementation. </param>
		/// <returns> A formatted string value if the value is not null; otherwise the default value specified. </returns>
		public string SafeToString<TValue>(TValue value, string format)
		{
			return this.SafeToString<TValue>(value, format, string.Empty);
		}

		/// <summary>
		/// Returns a string that represents the specified type with the format specification. If the value is null, then the default value is returned. No trimming is performed.
		/// </summary>
		/// <typeparam name="TValue"> The type of the value to obtain a string representation. </typeparam>
		/// <param name="value"> The target value. </param>
		/// <param name="format"> The string specifying the format to use or null to use the default format defined for the type of the IFormattable implementation. </param>
		/// <param name="default"> The default value to return if the value is null. </param>
		/// <returns> A formatted string value if the value is not null; otherwise the default value specified. </returns>
		public string SafeToString<TValue>(TValue value, string format, string @default)
		{
			return this.SafeToString<TValue>(value, format, @default, false);
		}

		/// <summary>
		/// Returns a string that represents the specified type with the format specification. If the value is null, then the default value is returned. No trimming is performed.
		/// </summary>
		/// <typeparam name="TValue"> The type of the value to obtain a string representation. </typeparam>
		/// <param name="value"> The target value. </param>
		/// <param name="format"> The string specifying the format to use or null to use the default format defined for the type of the IFormattable implementation. </param>
		/// <param name="default"> The default value to return if the value is null. </param>
		/// <param name="dofvisnow"> Use default value if the formatted value is null or whotespace. </param>
		/// <returns> A formatted string value if the value is not null; otherwise the default value specified. </returns>
		public string SafeToString<TValue>(TValue value, string format, string @default, bool dofvisnow)
		{
			string retval;

			//@default = (@default ?? string.Empty);

			if ((object)value == null)
				return @default;

			if (value is IFormattable)
				retval = ((IFormattable)value).ToString(format, null);
			else
				retval = value.ToString();

			if (this.IsNullOrWhiteSpace(retval) && dofvisnow)
				retval = @default;

			return retval;
		}

		/// <summary>
		/// Converts the specified string representation to its valueType equivalent and returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="valueType"> The type to convert the string value. </param>
		/// <param name="value"> A string containing a valueType to convert. </param>
		/// <param name="result"> When this method returns, contains the valueType value equivalent contained in value, if the conversion succeeded, or null if the conversion failed. The conversion fails if the value parameter is null, is an empty string, or does not contain a valid string representation of an valueType. This parameter is passed uninitialized. </param>
		/// <returns> A boolean value of true if the value was converted successfully; otherwise, false. </returns>
		public bool TryParse(Type valueType, string value, out object result)
		{
			bool retval;
			Type openNullableType;

			if ((object)valueType == null)
				throw new ArgumentNullException(nameof(valueType));

			openNullableType = typeof(Nullable<>);
			result = null;

			if (valueType.IsGenericType &&
				!valueType.IsGenericTypeDefinition &&
				valueType.GetGenericTypeDefinition().Equals(openNullableType))
			{
				if ((object)value == null)
					return true;
				else
					return this.TryParse(valueType.GetGenericArguments()[0], value, out result);
			}
			else if (valueType == typeof(DBNull))
			{
				// all values become DBNull
				retval = true;
				result = DBNull.Value;
			}
			else if (valueType == typeof(String))
			{
				retval = true;
				result = value;
			}
			else if (valueType == typeof(Boolean))
			{
				Boolean zresult;
				retval = Boolean.TryParse(value, out zresult);
				result = zresult;
				result = zresult;
			}
			else if (valueType == typeof(Byte))
			{
				Byte zresult;
				retval = Byte.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Char))
			{
				Char zresult;
				retval = Char.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(DateTime))
			{
				DateTime zresult;
				retval = DateTime.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(DateTimeOffset))
			{
				DateTimeOffset zresult;
				retval = DateTimeOffset.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Decimal))
			{
				Decimal zresult;
				retval = Decimal.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Double))
			{
				Double zresult;
				retval = Double.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Guid))
			{
				Guid zresult;
				retval = Guid.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Int16))
			{
				Int16 zresult;
				retval = Int16.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Int32))
			{
				Int32 zresult;
				retval = Int32.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Int64))
			{
				Int64 zresult;
				retval = Int64.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(SByte))
			{
				SByte zresult;
				retval = SByte.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Single))
			{
				Single zresult;
				retval = Single.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(TimeSpan))
			{
				TimeSpan zresult;
				retval = TimeSpan.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(UInt16))
			{
				UInt16 zresult;
				retval = UInt16.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(UInt32))
			{
				UInt32 zresult;
				retval = UInt32.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(UInt64))
			{
				UInt64 zresult;
				retval = UInt64.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Version))
			{
				Version zresult;
				retval = Version.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType.IsEnum) // special case
			{
				object zresult = null;
				retval = false;

				if (Enum.IsDefined(valueType, value))
				{
					zresult = Enum.Parse(valueType, value);
					retval = true;
				}

				result = zresult;
			}
			else
				throw new ArgumentOutOfRangeException(nameof(valueType), string.Format("The value type '{0}' is not supported.", valueType.FullName));

			return retval;
		}

		/// <summary>
		/// Converts the specified string representation to its TValue equivalent and returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TValue"> The type to parse the string value. </typeparam>
		/// <param name="value"> A string containing a TValue to convert. </param>
		/// <param name="result"> When this method returns, contains the TValue value equivalent contained in value, if the conversion succeeded, or default(TValue) if the conversion failed. The conversion fails if the value parameter is null, is an empty string, or does not contain a valid string representation of a TValue. This parameter is passed uninitialized. </param>
		/// <returns> A boolean value of true if the value was converted successfully; otherwise, false. </returns>
		public bool TryParse<TValue>(string value, out TValue result)
		{
			Type valueType;
			object oresult;
			bool retval;

			valueType = typeof(TValue);

			// no null check is intentional
			retval = this.TryParse(valueType, value, out oresult);

			if ((object)oresult == null)
				result = default(TValue);
			else
				result = (TValue)oresult;

			return retval;
		}

		#endregion
	}
}