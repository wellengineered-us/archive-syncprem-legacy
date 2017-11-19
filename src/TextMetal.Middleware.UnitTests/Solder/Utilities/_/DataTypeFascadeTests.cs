/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Utilities;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Utilities._
{
	/// <summary>
	/// Unit tests.
	/// </summary>
	[TestFixture]
	public class DataTypeFascadeTests
	{
		#region Constructors/Destructors

		public DataTypeFascadeTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCheckIsNullOrEmptyTest()
		{
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsTrue(dataTypeFascade.IsNullOrEmpty(null));
			Assert.IsTrue(dataTypeFascade.IsNullOrEmpty(string.Empty));
			Assert.IsFalse(dataTypeFascade.IsNullOrEmpty("   "));
			Assert.IsFalse(dataTypeFascade.IsNullOrEmpty("daniel"));
			Assert.IsFalse(dataTypeFascade.IsNullOrEmpty("   daniel     "));
		}

		[Test]
		public void ShouldCheckIsNullOrWhiteSpaceTest()
		{
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsTrue(dataTypeFascade.IsNullOrWhiteSpace(null));
			Assert.IsTrue(dataTypeFascade.IsNullOrWhiteSpace(string.Empty));
			Assert.IsTrue(dataTypeFascade.IsNullOrWhiteSpace("   "));
			Assert.IsFalse(dataTypeFascade.IsNullOrWhiteSpace("daniel"));
			Assert.IsFalse(dataTypeFascade.IsNullOrWhiteSpace("   daniel     "));
		}

		[Test]
		public void ShouldCheckIsWhiteSpaceTest()
		{
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(dataTypeFascade.IsWhiteSpace(null));
			Assert.IsTrue(dataTypeFascade.IsWhiteSpace("   "));
			Assert.IsFalse(dataTypeFascade.IsWhiteSpace("daniel"));
			Assert.IsFalse(dataTypeFascade.IsWhiteSpace("   daniel     "));
		}

		//[Test]
		//public void ShouldCheckObjectsCompareValueSemanticsTest()
		//{
		//    int? result;
		//    object objA, objB;

		//    // both null
		//    objA = null;
		//    objB = null;
		//    result = dataTypeFascade.ObjectsCompareValueSemantics(objA, objB);
		//    Assert.IsNull(result);

		//    // objA null, objB not null
		//    objA = null;
		//    objB = 10;
		//    result = dataTypeFascade.ObjectsCompareValueSemantics(objA, objB);
		//    Assert.Less(result, 0);

		//    // objA not null, objB null
		//    objA = 10;
		//    objB = null;
		//    result = dataTypeFascade.ObjectsCompareValueSemantics(objA, objB);
		//    Assert.Greater(result, 0);

		//    // objA == objB
		//    objA = 100;
		//    objB = 100;
		//    result = dataTypeFascade.ObjectsCompareValueSemantics(objA, objB);
		//    Assert.AreEqual(0, result);

		//    // objA != objB
		//    objA = 100;
		//    objB = -100;
		//    result = dataTypeFascade.ObjectsCompareValueSemantics(objA, objB);
		//    Assert.Greater(result, 0);
		//}

		[Test]
		public void ShouldCheckObjectsEqualValueSemanticsTest()
		{
			bool result;
			object objA, objB;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			// both null
			objA = null;
			objB = null;
			result = dataTypeFascade.ObjectsEqualValueSemantics(objA, objB);
			Assert.IsTrue(result);

			// objA null, objB not null
			objA = null;
			objB = "not null string";
			result = dataTypeFascade.ObjectsEqualValueSemantics(objA, objB);
			Assert.IsFalse(result);

			// objA not null, objB null
			objA = "not null string";
			objB = null;
			result = dataTypeFascade.ObjectsEqualValueSemantics(objA, objB);
			Assert.IsFalse(result);

			// objA == objB
			objA = 100;
			objB = 100;
			result = dataTypeFascade.ObjectsEqualValueSemantics(objA, objB);
			Assert.IsTrue(result);

			// objA != objB
			objA = 100;
			objB = -100;
			result = dataTypeFascade.ObjectsEqualValueSemantics(objA, objB);
			Assert.IsFalse(result);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnInvalidGenericTypeTryParseTest()
		{
			KeyValuePair<int, int> ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<KeyValuePair<int, int>>(DBNull.Value.ToString(), out ovalue);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnInvalidTypeTryParseTest()
		{
			object ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse(typeof(KeyValuePair<int, int>), DBNull.Value.ToString(), out ovalue);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeChangeTypeTest()
		{
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			dataTypeFascade.ChangeType(1, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeDefaultValueTest()
		{
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			dataTypeFascade.DefaultValue(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeTryParseTest()
		{
			object ovalue;
			bool result;

			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse(null, string.Empty, out ovalue);
		}

		[Test]
		public void ShouldGetBooleanTest()
		{
			Boolean ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Boolean>("true", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(true, ovalue);

			result = dataTypeFascade.TryParse<Boolean>("false", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(false, ovalue);
		}

		[Test]
		public void ShouldGetByteTest()
		{
			Byte ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Byte>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetCharTest()
		{
			Char ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Char>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual('0', ovalue);
		}

		[Test]
		public void ShouldGetDateTimeOffsetTest()
		{
			DateTimeOffset ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<DateTimeOffset>("6/22/2003", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new DateTimeOffset(new DateTime(2003, 6, 22)), ovalue);
		}

		[Test]
		public void ShouldGetDateTimeTest()
		{
			DateTime ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<DateTime>("6/22/2003", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new DateTime(2003, 6, 22), ovalue);
		}

		[Test]
		public void ShouldGetDBNullTest()
		{
			DBNull ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<DBNull>("", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(DBNull.Value, ovalue);

			result = dataTypeFascade.TryParse<DBNull>("___", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(DBNull.Value, ovalue);
		}

		[Test]
		public void ShouldGetDecimalTest()
		{
			Decimal ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Decimal>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetDefaultOnNullValueChangeTypeTest()
		{
			object value;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			value = dataTypeFascade.ChangeType(null, typeof(int));

			Assert.AreEqual(default(int), value);
		}

		[Test]
		public void ShouldGetDefaultOnNullValueChangeTypeTick1Test()
		{
			int value;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			value = dataTypeFascade.ChangeType<int>(null);

			Assert.AreEqual(default(int), value);
		}

		[Test]
		public void ShouldGetDefaultValueTest()
		{
			object defaultValue;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			defaultValue = dataTypeFascade.DefaultValue(typeof(int));

			Assert.AreEqual(0, defaultValue);

			defaultValue = dataTypeFascade.DefaultValue(typeof(int?));

			Assert.IsNull(defaultValue);
		}

		[Test]
		public void ShouldGetDoubleTest()
		{
			Double ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Double>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetEnumTest()
		{
			CharSet ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<CharSet>("Unicode", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(CharSet.Unicode, ovalue);
		}

		[Test]
		public void ShouldGetGuidTest()
		{
			Guid ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Guid>("{00000000-0000-0000-0000-000000000000}", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(Guid.Empty, ovalue);
		}

		[Test]
		public void ShouldGetInt16Test()
		{
			Int16 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Int16>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetInt32Test()
		{
			Int32 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Int32>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetInt64Test()
		{
			Int64 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Int64>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueChangeTypeTest()
		{
			object value;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			value = dataTypeFascade.ChangeType((byte)1, typeof(int));

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int>(value);
			Assert.AreEqual((int)1, value);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueChangeTypeTick1Test()
		{
			int value;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			value = dataTypeFascade.ChangeType<int>((byte)1);

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int>(value);
			Assert.AreEqual((int)1, value);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueNullableChangeTypeTest()
		{
			object value;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			value = dataTypeFascade.ChangeType((byte)1, typeof(int?));

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int?>(value);
			Assert.AreEqual((int?)1, value);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueNullableChangeTypeTick1Test()
		{
			int? value;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			value = dataTypeFascade.ChangeType<int?>((byte)1);

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int?>(value);
			Assert.AreEqual((int?)1, value);
		}

		[Test]
		public void ShouldGetSByteTest()
		{
			SByte ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<SByte>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetSingleTest()
		{
			Single ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Single>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetStringTest()
		{
			String ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<String>("0-8-8-8-8-8-8-8-c", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual("0-8-8-8-8-8-8-8-c", ovalue);
		}

		[Test]
		public void ShouldGetTimeSpanTest()
		{
			TimeSpan ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<TimeSpan>("0:0:0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(TimeSpan.Zero, ovalue);
		}

		[Test]
		public void ShouldGetUInt16Test()
		{
			UInt16 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<UInt16>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetUInt32Test()
		{
			UInt32 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<UInt32>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetUInt64Test()
		{
			UInt64 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<UInt64>("0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetVersionTest()
		{
			Version ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<Version>("1.2.3.4", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new Version(1, 2, 3, 4), ovalue);

			result = dataTypeFascade.TryParse<Version>("0.0.0.0", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new Version(0, 0, 0, 0), ovalue);
		}

		[Test]
		public void ShouldNotGetBooleanTest()
		{
			Boolean ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Boolean>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetByteTest()
		{
			Byte ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Byte>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Byte>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetCharTest()
		{
			Char ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Char>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetDateTimeOffsetTest()
		{
			DateTimeOffset ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<DateTimeOffset>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetDateTimeTest()
		{
			DateTime ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<DateTime>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetDecimalTest()
		{
			Decimal ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Decimal>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Decimal>("11111111111111111111111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetDoubleTest()
		{
			Double ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Double>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Double>("999,769,313,486,232,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000.00", out ovalue));
		}

		[Test]
		public void ShouldNotGetEnumTest()
		{
			CharSet ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<CharSet>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetGuidTest()
		{
			Guid ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Guid>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetInt16Test()
		{
			Int16 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Int16>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Int16>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetInt32Test()
		{
			Int32 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Int32>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Int32>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetInt64Test()
		{
			Int64 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Int64>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Int64>("9999999999999999999", out ovalue));
		}

		[Test]
		public void ShouldNotGetSByteTest()
		{
			SByte ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<SByte>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<SByte>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetSingleTest()
		{
			Single ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Single>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<Single>("999,282,300,000,000,000,000,000,000,000,000,000,000.00", out ovalue));
		}

		[Test]
		public void ShouldNotGetTimeSpanTest()
		{
			TimeSpan ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<TimeSpan>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<TimeSpan>("99999999.02:48:05.4775807", out ovalue));
		}

		[Test]
		public void ShouldNotGetUInt16Test()
		{
			UInt16 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt16>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt16>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetUInt32Test()
		{
			UInt32 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt32>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt32>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetUInt64Test()
		{
			UInt64 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt64>("gibberish", out ovalue));
			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt64>("99999999999999999999", out ovalue));
		}

		[Test]
		public void ShouldPreventNullCoaleseTest()
		{
			bool result;
			object objA, objB;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			// both null
			objA = null;
			objB = (int)0;
			result = dataTypeFascade.ObjectsEqualValueSemantics(objA, objB);
			Assert.IsFalse(result);
		}

		[Test]
		public void ShouldSafeToStringTest()
		{
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.AreEqual("123.456", dataTypeFascade.SafeToString(123.456));
			Assert.AreEqual("123.46", dataTypeFascade.SafeToString(123.456, "n"));
			Assert.AreEqual("urn:foo", dataTypeFascade.SafeToString(new Uri("urn:foo"), "n"));

			Assert.AreEqual(string.Empty, dataTypeFascade.SafeToString((object)null, null));
			Assert.AreEqual(null, dataTypeFascade.SafeToString((object)null, null, null));
			Assert.AreEqual("1", dataTypeFascade.SafeToString((object)string.Empty, null, "1", true));
		}

		[Test]
		public void ShouldSpecialGetValueOnNonNullNullableGenericTryParseTest()
		{
			int? ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<int?>("100", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)100, ovalue);
		}

		[Test]
		public void ShouldSpecialGetValueOnNonNullNullableTryParseTest()
		{
			object ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse(typeof(int?), "100", out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)100, ovalue);
		}

		[Test]
		public void ShouldSpecialGetValueOnNullNullableGenericTryParseTest()
		{
			int? ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse<int?>(null, out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)null, ovalue);
		}

		[Test]
		public void ShouldSpecialGetValueOnNullNullableTryParseTest()
		{
			object ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			result = dataTypeFascade.TryParse(typeof(int?), null, out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)null, ovalue);
		}

		[Test]
		public void ShouldWithNullCanGetDBNullTest()
		{
			DBNull ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsTrue(result = dataTypeFascade.TryParse<DBNull>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetBooleanTest()
		{
			Boolean ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Boolean>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetByteTest()
		{
			Byte ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Byte>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetCharTest()
		{
			Char ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Char>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDateTimeOffsetTest()
		{
			DateTimeOffset ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<DateTimeOffset>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDateTimeTest()
		{
			DateTime ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<DateTime>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDecimalTest()
		{
			Decimal ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Decimal>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDoubleTest()
		{
			Double ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Double>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetEnumTest()
		{
			CharSet ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<CharSet>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetGuidTest()
		{
			Guid ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Guid>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetInt16Test()
		{
			Int16 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Int16>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetInt32Test()
		{
			Int32 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Int32>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetInt64Test()
		{
			Int64 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Int64>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetSByteTest()
		{
			SByte ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<SByte>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetSingleTest()
		{
			Single ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Single>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetTimeSpanTest()
		{
			TimeSpan ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<TimeSpan>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetUInt16Test()
		{
			UInt16 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt16>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetUInt32Test()
		{
			UInt32 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt32>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetUInt64Test()
		{
			UInt64 ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<UInt64>(null, out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetVersionTest()
		{
			Version ovalue;
			bool result;
			DataTypeFascade dataTypeFascade;

			dataTypeFascade = new DataTypeFascade();

			Assert.IsFalse(result = dataTypeFascade.TryParse<Version>(null, out ovalue));
		}

		#endregion
	}
}