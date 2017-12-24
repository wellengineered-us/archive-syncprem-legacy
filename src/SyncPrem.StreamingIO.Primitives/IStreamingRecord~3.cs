/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.StreamingIO.Primitives.Schema;

namespace SyncPrem.StreamingIO.Primitives
{
	public interface IStreamingRecord
	{
		object Key
		{
			get;
		}

		object Value
		{
			get;
		}

		ISchema KeySchema
		{
			get;
		}

		ISchema ValueSchema
		{
			get;
		}

		DateTimeOffset Timestamp
		{
			get;
		}

		Guid Topic
		{
			get;
		}

		UInt64 Partition
		{
			get;
		}

		object Offset
		{
			get;
		}
	}

	public interface IStreamingRecord<out TKey, out TValue, out TOffset> : IStreamingRecord
	{
		new TKey Key
		{
			get;
		}

		new TValue Value
		{
			get;
		}

		new TOffset Offset
		{
			get;
		}
	}
}