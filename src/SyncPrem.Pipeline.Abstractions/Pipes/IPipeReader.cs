﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Pipeline.Abstractions.Pipes
{
	public interface IPipeReader
	{
		#region Methods/Operators

		void Read(IPipe pipe);

		#endregion
	}
}