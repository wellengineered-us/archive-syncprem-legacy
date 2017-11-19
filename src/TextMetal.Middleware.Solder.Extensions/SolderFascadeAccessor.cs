/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.Solder.Utilities;

namespace TextMetal.Middleware.Solder.Extensions
{
	public static class SolderFascadeAccessor
	{
		#region Fields/Constants

		private static readonly Lazy<IAdoNetBufferingFascade> adoNetStreamingFascadeFactory = new Lazy<IAdoNetBufferingFascade>(() => new AdoNetBufferingFascade(DataTypeFascade));
		private static readonly Lazy<IDataTypeFascade> dataTypeFascadeFactory = new Lazy<IDataTypeFascade>(() => new DataTypeFascade());
		private static readonly Lazy<IReflectionFascade> reflectionFascadeFactory = new Lazy<IReflectionFascade>(() => new ReflectionFascade(DataTypeFascade));

		#endregion

		#region Properties/Indexers/Events

		public static IAdoNetBufferingFascade AdoNetBufferingFascade
		{
			get
			{
				return AdoNetBufferingFascadeFactory.Value;
			}
		}

		private static Lazy<IAdoNetBufferingFascade> AdoNetBufferingFascadeFactory
		{
			get
			{
				return adoNetStreamingFascadeFactory;
			}
		}

		public static IDataTypeFascade DataTypeFascade
		{
			get
			{
				return DataTypeFascadeFactory.Value;
			}
		}

		private static Lazy<IDataTypeFascade> DataTypeFascadeFactory
		{
			get
			{
				return dataTypeFascadeFactory;
			}
		}

		public static IReflectionFascade ReflectionFascade
		{
			get
			{
				return ReflectionFascadeFactory.Value;
			}
		}

		private static Lazy<IReflectionFascade> ReflectionFascadeFactory
		{
			get
			{
				return reflectionFascadeFactory;
			}
		}

		#endregion
	}
}