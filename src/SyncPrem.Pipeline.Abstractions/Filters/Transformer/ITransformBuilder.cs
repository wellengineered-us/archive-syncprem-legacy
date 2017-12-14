﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Pipeline.Abstractions.Filters.Transformer
{
	public interface ITransformBuilder
	{
		#region Methods/Operators

		TransformDelegate Build();

		ITransformBuilder New();

		ITransformBuilder Use(Func<TransformDelegate, TransformDelegate> middleware);

		#endregion
	}
}