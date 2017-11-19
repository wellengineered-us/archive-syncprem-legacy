/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using NMock;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Utilities;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Utilities._
{
	[TestFixture]
	public class AssemblyInformationFascadeTests
	{
		#region Constructors/Destructors

		public AssemblyInformationFascadeTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			AssemblyInformationFascade assemblyInformationFascade;
			MockFactory mockFactory;
			IReflectionFascade mockReflectionFascade;
			Assembly mockAssembly, _unusedAssembly = null;

			const string ASSEMBLY_TITLE = "sdfgsfdg";
			const string ASSEMBLY_DESCRIPTION = "rthrs";
			const string ASSEMBLY_PRODUCT = "hnfgnhfghn";
			const string ASSEMBLY_COPYRIGHT = "bxcbx";
			const string ASSEMBLY_COMPANY = "swergwerg";
			const string ASSEMBLY_CONFIGURATION = "webxvxn";
			const string ASSEMBLY_FILE_VERSION = "wegwegr";
			const string ASSEMBLY_INFORMATIONAL_VERSION = "dnxnn";
			const string ASSEMBLY_TRADEMARK = "yjtmrum";

			mockFactory = new MockFactory();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();
			mockAssembly = Assembly.GetEntryAssembly(); // dummy

			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyTitleAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyTitleAttribute(ASSEMBLY_TITLE));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyDescriptionAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyDescriptionAttribute(ASSEMBLY_DESCRIPTION));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyProductAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyProductAttribute(ASSEMBLY_PRODUCT));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyCopyrightAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyCopyrightAttribute(ASSEMBLY_COPYRIGHT));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyCompanyAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyCompanyAttribute(ASSEMBLY_COMPANY));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyConfigurationAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyConfigurationAttribute(ASSEMBLY_CONFIGURATION));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyFileVersionAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyFileVersionAttribute(ASSEMBLY_FILE_VERSION));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyInformationalVersionAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyInformationalVersionAttribute(ASSEMBLY_INFORMATIONAL_VERSION));
			Expect.On(mockReflectionFascade).One.Method(m => m.GetOneAttribute<AssemblyTrademarkAttribute>(_unusedAssembly)).With(mockAssembly).WillReturn(new AssemblyTrademarkAttribute(ASSEMBLY_TRADEMARK));

			assemblyInformationFascade = new AssemblyInformationFascade(mockReflectionFascade, mockAssembly);

			Assert.AreEqual(ASSEMBLY_TITLE, assemblyInformationFascade.Title);
			Assert.AreEqual(ASSEMBLY_DESCRIPTION, assemblyInformationFascade.Description);
			Assert.AreEqual(ASSEMBLY_PRODUCT, assemblyInformationFascade.Product);
			Assert.AreEqual(ASSEMBLY_COPYRIGHT, assemblyInformationFascade.Copyright);
			Assert.AreEqual(ASSEMBLY_COMPANY, assemblyInformationFascade.Company);
			Assert.AreEqual(ASSEMBLY_CONFIGURATION, assemblyInformationFascade.Configuration);
			Assert.AreEqual(ASSEMBLY_FILE_VERSION, assemblyInformationFascade.NativeFileVersion);
			Assert.AreEqual(ASSEMBLY_INFORMATIONAL_VERSION, assemblyInformationFascade.InformationalVersion);
			Assert.AreEqual(ASSEMBLY_TRADEMARK, assemblyInformationFascade.Trademark);
			Assert.AreEqual(mockAssembly.GetName().Version.ToString(), assemblyInformationFascade.AssemblyVersion);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullAssemblyCreateTest()
		{
			AssemblyInformationFascade assemblyInformationFascade;
			MockFactory mockFactory;
			IReflectionFascade mockReflectionFascade;
			Assembly mockAssembly;

			mockFactory = new MockFactory();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();
			mockAssembly = null;

			assemblyInformationFascade = new AssemblyInformationFascade(mockReflectionFascade, mockAssembly);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullReflectionFascadeCreateTest()
		{
			AssemblyInformationFascade assemblyInformationFascade;
			MockFactory mockFactory;
			IReflectionFascade mockReflectionFascade;
			Assembly mockAssembly;

			mockFactory = new MockFactory();
			mockReflectionFascade = null;
			mockAssembly = Assembly.GetEntryAssembly(); // dummy

			assemblyInformationFascade = new AssemblyInformationFascade(mockReflectionFascade, mockAssembly);
		}

		#endregion
	}
}