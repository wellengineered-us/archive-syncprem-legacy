/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.DataMasking
{
	public interface IColumnSpec<TObfuscationStrategySpec> : IColumnSpec
		where TObfuscationStrategySpec : class, IObfuscationStrategySpec
	{
		#region Properties/Indexers/Events

		new TObfuscationStrategySpec ObfuscationStrategySpec
		{
			get;
			set;
		}

		#endregion
	}
}