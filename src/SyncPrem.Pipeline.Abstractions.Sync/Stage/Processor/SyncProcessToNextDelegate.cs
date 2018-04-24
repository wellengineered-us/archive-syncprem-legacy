/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor
{
	public delegate ISyncChannel SyncProcessToNextDelegate(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel, SyncProcessDelegate next);
}