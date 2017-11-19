/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Runtime.InteropServices;

using Microsoft.Extensions.Configuration;

using NMock;
using NMock.Actions;
using NMock.Matchers;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Utilities;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Utilities._
{
	[TestFixture]
	public class AppConfigFascadeTests
	{
		#region Constructors/Destructors

		public AppConfigFascadeTests()
		{
		}

		#endregion

		#region Fields/Constants

		private const string UNATTAINABLE_VALUE = "!@#$%^&*(";

		#endregion

		#region Methods/Operators

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnAppSettingsConfigurationRootCreateTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockConfigurationRoot = null;
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnDataTypeFascadeCreateTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = null;

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueBoolean";
			string __unusedString = null;
			Boolean __unusedBoolean;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Boolean>(__unusedString, out __unusedBoolean)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Boolean>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueByte";
			string __unusedString = null;
			Byte __unusedByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Byte>(__unusedString, out __unusedByte)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Byte>("BadAppConfigFascadeValueByte");
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueChar";
			string __unusedString = null;
			Char __unusedChar;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Char>(__unusedString, out __unusedChar)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Char>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueDateTime";
			string __unusedString = null;
			DateTime __unusedDateTime;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<DateTime>(__unusedString, out __unusedDateTime)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<DateTime>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueDecimal";
			string __unusedString = null;
			Decimal __unusedDecimal;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Decimal>(__unusedString, out __unusedDecimal)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Decimal>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueDouble";
			string __unusedString = null;
			Double __unusedDouble;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Double>(__unusedString, out __unusedDouble)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Double>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueEnum";
			string __unusedString = null;
			CharSet __unusedCharSet;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<CharSet>(__unusedString, out __unusedCharSet)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<CharSet>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueGuid";
			string __unusedString = null;
			Guid __unusedGuid;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Guid>(__unusedString, out __unusedGuid)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Guid>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueInt16";
			string __unusedString = null;
			Int16 __unusedInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Int16>(__unusedString, out __unusedInt16)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueInt32";
			string __unusedString = null;
			Int32 __unusedInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Int32>(__unusedString, out __unusedInt32)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueInt64";
			string __unusedString = null;
			Int64 __unusedInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Int64>(__unusedString, out __unusedInt64)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueSByte";
			string __unusedString = null;
			SByte __unusedSByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<SByte>(__unusedString, out __unusedSByte)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<SByte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueSingle";
			string __unusedString = null;
			Single __unusedSingle;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Single>(__unusedString, out __unusedSingle)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Single>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueTimeSpan";
			string __unusedString = null;
			TimeSpan __unusedTimeSpan;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<TimeSpan>(__unusedString, out __unusedTimeSpan)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<TimeSpan>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueUInt16";
			string __unusedString = null;
			UInt16 __unusedUInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<UInt16>(__unusedString, out __unusedUInt16)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueUInt32";
			string __unusedString = null;
			UInt32 __unusedUInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<UInt32>(__unusedString, out __unusedUInt32)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetTypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueUInt64";
			string __unusedString = null;
			UInt64 __unusedUInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<UInt64>(__unusedString, out __unusedUInt64)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueBoolean";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedBoolean;
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedBoolean)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueByte";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedByte;
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedByte)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueChar";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedChar;
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedChar)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueDateTime";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDateTime;
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedDateTime)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueDecimal";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDecimal;
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedDecimal)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueDouble";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDouble;
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedDouble)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueEnum";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedCharSet;
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedCharSet)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueGuid";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedGuid;
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedGuid)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueInt16";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt16;
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedInt16)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueInt32";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt32;
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedInt32)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueInt64";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt64;
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedInt64)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueSByte";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSByte;
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedSByte)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueSingle";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSingle;
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedSingle)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueTimeSpan";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedTimeSpan;
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedTimeSpan)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueUInt16";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt16;
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedUInt16)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueUInt32";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt32;
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedUInt32)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnInvalidValueGetUntypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "BadAppConfigFascadeValueUInt64";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt64;
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedUInt64)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).WillReturn(false);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueBoolean";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Boolean>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueByte";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Byte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueChar";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Char>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueDateTime";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<DateTime>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueDecimal";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Decimal>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueDouble";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Double>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueEnum";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<CharSet>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueGuid";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Guid>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueInt16";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueInt32";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueInt64";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueSByte";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<SByte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueSingle";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Single>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueTimeSpan";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<TimeSpan>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueUInt16";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueUInt32";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetTypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueUInt64";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueBoolean";
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueByte";
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueChar";
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueDateTime";
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueDecimal";
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueDouble";
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueEnum";
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueGuid";
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueInt16";
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueInt32";
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueInt64";
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueSByte";
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueSingle";
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueTimeSpan";
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueUInt16";
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueUInt32";
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppConfigException))]
		public void ShouldFailOnNonExistKeyGetUntypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "NotThereAppConfigFascadeValueUInt64";
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Boolean>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Byte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Char>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<DateTime>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Decimal>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Double>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<CharSet>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Guid>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Int64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<SByte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<Single>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<TimeSpan>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<UInt64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyHasAppSettingTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.HasAppSetting(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullNameGetTypedAppSettingTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting<string>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullNameGetUntypedAppSettingTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(String);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeGetUntypedAppSettingTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "";
			Type valueType = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			appConfigFascade.GetAppSetting(valueType, KEY);
		}

		//[Test]
		//public void Should__GetArgsParseCommandLineArgumentsTest()
		//{
		//	List<string> args;
		//	IDictionary<string, IList<string>> cmdlnargs;

		//	args = new List<string>();
		//	args.Add("arg0");
		//	args.Add(string.Empty);
		//	args.Add("-");
		//	args.Add("-arg1");
		//	args.Add("-arg2:");
		//	args.Add("-arg3:");
		//	args.Add("-arg4:value4");
		//	args.Add("-arg5:value5");
		//	args.Add("-arg4:value4");
		//	args.Add("arg6:value6");
		//	args.Add("-:value7");
		//	args.Add("-arg8:value8a");
		//	args.Add("-arg8:value8b");
		//	args.Add("-arg8:value8c");
		//	args.Add("-arg8:value8a");

		//	cmdlnargs = AppConfigFascade.ReflectionFascade.ParseCommandLineArguments(args.ToArray());

		//	Assert.IsNotNull(cmdlnargs);
		//	Assert.AreEqual(3, cmdlnargs.Count);

		//	Assert.AreEqual(1, cmdlnargs["arg4"].Count);
		//	Assert.AreEqual("value4", cmdlnargs["arg4"][0]);

		//	Assert.AreEqual(1, cmdlnargs["arg5"].Count);
		//	Assert.AreEqual("value5", cmdlnargs["arg5"][0]);

		//	Assert.AreEqual(3, cmdlnargs["arg8"].Count);
		//	Assert.AreEqual("value8a", cmdlnargs["arg8"][0]);
		//	Assert.AreEqual("value8b", cmdlnargs["arg8"][1]);
		//	Assert.AreEqual("value8c", cmdlnargs["arg8"][2]);
		//}

		[Test]
		public void ShouldGetTypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueBoolean";
			Boolean expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Boolean __unusedBoolean;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = true;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Boolean>(__unusedString, out __unusedBoolean)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Boolean>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueByte";
			Byte expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Byte __unusedByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Byte>(__unusedString, out __unusedByte)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Byte>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueChar";
			Char expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Char __unusedChar;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = '\0';

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Char>(__unusedString, out __unusedChar)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Char>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueDateTime";
			DateTime expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			DateTime __unusedDateTime;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = DateTime.Now;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<DateTime>(__unusedString, out __unusedDateTime)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<DateTime>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueDecimal";
			Decimal expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Decimal __unusedDecimal;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Decimal>(__unusedString, out __unusedDecimal)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Decimal>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueDouble";
			Double expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Double __unusedDouble;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Double>(__unusedString, out __unusedDouble)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Double>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueEnum";
			CharSet expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			CharSet __unusedCharSet;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = CharSet.Unicode;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<CharSet>(__unusedString, out __unusedCharSet)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<CharSet>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueGuid";
			Guid expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Guid __unusedGuid;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = Guid.Empty;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Guid>(__unusedString, out __unusedGuid)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Guid>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueInt16";
			Int16 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Int16 __unusedInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Int16>(__unusedString, out __unusedInt16)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Int16>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueInt32";
			Int32 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Int32 __unusedInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Int32>(__unusedString, out __unusedInt32)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Int32>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueInt64";
			Int64 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Int64 __unusedInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Int64>(__unusedString, out __unusedInt64)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Int64>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueSByte";
			SByte expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			SByte __unusedSByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<SByte>(__unusedString, out __unusedSByte)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<SByte>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueSingle";
			Single expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Single __unusedSingle;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<Single>(__unusedString, out __unusedSingle)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<Single>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedStringTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueString";
			String expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			//String __unusedString;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = "foobar";

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<String>(__unusedString, out __unusedString)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<String>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueTimeSpan";
			TimeSpan expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			TimeSpan __unusedTimeSpan;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = TimeSpan.Zero;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<TimeSpan>(__unusedString, out __unusedTimeSpan)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<TimeSpan>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueUInt16";
			UInt16 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			UInt16 __unusedUInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<UInt16>(__unusedString, out __unusedUInt16)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<UInt16>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueUInt32";
			UInt32 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			UInt32 __unusedUInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<UInt32>(__unusedString, out __unusedUInt32)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<UInt32>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueUInt64";
			UInt64 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			UInt64 __unusedUInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse<UInt64>(__unusedString, out __unusedUInt64)).With(UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting<UInt64>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedBooleanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueBoolean";
			Boolean expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedBoolean;
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = true;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedBoolean)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueByte";
			Byte expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedByte;
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedByte)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedCharTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueChar";
			Char expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedChar;
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = '\0';

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedChar)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedDateTimeTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueDateTime";
			DateTime expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDateTime;
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = DateTime.Now;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedDateTime)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedDecimalTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueDecimal";
			Decimal expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDecimal;
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedDecimal)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedDoubleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueDouble";
			Double expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDouble;
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedDouble)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedEnumTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueEnum";
			CharSet expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedCharSet;
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = CharSet.Unicode;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedCharSet)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedGuidTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueGuid";
			Guid expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedGuid;
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = Guid.Empty;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedGuid)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueInt16";
			Int16 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt16;
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedInt16)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueInt32";
			Int32 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt32;
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedInt32)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueInt64";
			Int64 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt64;
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedInt64)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedSByteTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueSByte";
			SByte expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSByte;
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedSByte)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedSingleTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueSingle";
			Single expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSingle;
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedSingle)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedStringTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueString";
			String expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedString2;
			Type valueType = typeof(String);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = "foobar";

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedString2)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedTimeSpanTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueTimeSpan";
			TimeSpan expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedTimeSpan;
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = TimeSpan.Zero;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedTimeSpan)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedUInt16Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueUInt16";
			UInt16 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt16;
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedUInt16)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedUInt32Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueUInt32";
			UInt32 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt32;
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedUInt32)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedUInt64Test()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueUInt64";
			UInt64 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt64;
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = 0L;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			Expect.On(mockDataTypeFascade).One.Method(m => m.TryParse(__unusedType, __unusedString, out __unusedUInt64)).With(valueType, UNATTAINABLE_VALUE, new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("result", expected), Return.Value(true));

			value = appConfigFascade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldHaveFalseHasAppSettingTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueBooleanFalse";
			bool expected, value;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = false;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			value = appConfigFascade.HasAppSetting(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldHaveTrueHasAppSettingTest()
		{
			AppConfigFascade appConfigFascade;
			IConfigurationRoot mockConfigurationRoot;
			IDataTypeFascade mockDataTypeFascade;
			MockFactory mockFactory;
			const string KEY = "AppConfigFascadeValueBoolean";
			bool expected, value;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();
			mockDataTypeFascade = mockFactory.CreateInstance<IDataTypeFascade>();

			appConfigFascade = new AppConfigFascade(mockConfigurationRoot, mockDataTypeFascade);

			expected = true;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			value = appConfigFascade.HasAppSetting(KEY);

			Assert.AreEqual(expected, value);
		}

		#endregion
	}
}