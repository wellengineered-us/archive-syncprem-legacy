/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SyncPrem.StreamingIO.DataMasking.Strategies;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.StreamingIO.DataMasking
{
	public sealed class OxymoronEngine : IOxymoronEngine, IObfuscationContext
	{
		#region Constructors/Destructors

		public OxymoronEngine(ResolveDictionaryValueDelegate resolveDictionaryValueCallback, IObfuscationSpec obfuscationSpec)
		{
			if ((object)resolveDictionaryValueCallback == null)
				throw new ArgumentNullException(nameof(resolveDictionaryValueCallback));

			if ((object)obfuscationSpec == null)
				throw new ArgumentNullException(nameof(obfuscationSpec));

			this.resolveDictionaryValueCallback = resolveDictionaryValueCallback;
			this.obfuscationSpec = obfuscationSpec;

			//OnlyWhen._DEBUG_ThenPrint(string.Format("SECURITY_CRITICAL_OPTION: {0}={1}", nameof(obfuscationSpec.EnablePassThru), obfuscationSpec.EnablePassThru ?? false));

			// calling this more than once kills performance
			obfuscationSpec.AssertValid();
			//throw new InvalidOperationException(string.Format("Obfuscation configuration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));
		}

		#endregion

		#region Fields/Constants

		private const long DEFAULT_HASH_BUCKET_SIZE = long.MaxValue;
		private const bool SUBSTITUTION_CACHE_ENABLED = true;
		private readonly IDictionary<long, IColumnSpec> fieldCache = new Dictionary<long, IColumnSpec>();
		private readonly IObfuscationSpec obfuscationSpec;
		private readonly IDictionary<string, IObfuscationStrategy> obfuscationStrategyCache = new Dictionary<string, IObfuscationStrategy>();
		private readonly ResolveDictionaryValueDelegate resolveDictionaryValueCallback;
		private readonly IDictionary<string, IDictionary<object, object>> substitutionCacheRoot = new Dictionary<string, IDictionary<object, object>>();
		private bool disposed;

		#endregion

		#region Properties/Indexers/Events

		private IDictionary<long, IColumnSpec> FieldCache
		{
			get
			{
				return this.fieldCache;
			}
		}

		public IObfuscationSpec ObfuscationSpec
		{
			get
			{
				return this.obfuscationSpec;
			}
		}

		private IDictionary<string, IObfuscationStrategy> ObfuscationStrategyCache
		{
			get
			{
				return this.obfuscationStrategyCache;
			}
		}

		private ResolveDictionaryValueDelegate ResolveDictionaryValueCallback
		{
			get
			{
				return this.resolveDictionaryValueCallback;
			}
		}

		private IDictionary<string, IDictionary<object, object>> SubstitutionCacheRoot
		{
			get
			{
				return this.substitutionCacheRoot;
			}
		}

		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
			private set
			{
				this.disposed = value;
			}
		}

		#endregion

		#region Methods/Operators

		private static long? GetHash(long? multiplier, long? size, long? seed, object value)
		{
			const long DEFAULT_HASH = -1L;
			long hashCode;
			byte[] buffer;
			Type valueType;
			string _value;

			if ((object)multiplier == null)
				return null;

			if ((object)size == null)
				return null;

			if ((object)seed == null)
				return null;

			if (size == 0L)
				return null; // prevent DIV0

			if ((object)value == null)
				return null;

			valueType = value.GetType();

			if (valueType != typeof(String))
				return null;

			_value = (String)value;

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(_value))
				return DEFAULT_HASH;

			_value = _value.Trim();

			buffer = Encoding.UTF8.GetBytes(_value);

			hashCode = (long)seed;
			for (int index = 0; index < buffer.Length; index++)
				hashCode = ((long)multiplier * hashCode + buffer[index]) % uint.MaxValue;

			if (hashCode > int.MaxValue)
				hashCode = hashCode - uint.MaxValue;

			if (hashCode < 0)
				hashCode = hashCode + int.MaxValue;

			hashCode = (hashCode % (long)size);

			return (int)hashCode;
		}

		public static IEnumerable<IRecord> RecordsFromJsonFile(string jsonFilePath)
		{
			IEnumerable<IRecord> records;

			records = JsonSerializationStrategy.Instance.GetObjectFromFile<Record[]>(jsonFilePath);

			return records;
		}

		public static void RecordsToJsonFile(IEnumerable<IRecord> records, string jsonFilePath)
		{
			JsonSerializationStrategy.Instance.SetObjectToFile<IEnumerable<IRecord>>(jsonFilePath, records);
		}

		private object _GetObfuscatedValue(IField field, object originalFieldValue)
		{
			IObfuscationStrategy obfuscationStrategy;
			IColumnSpec columnSpec;
			object obfuscatedFieldValue;

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if (!this.FieldCache.TryGetValue(field.FieldIndex, out columnSpec))
			{
				columnSpec = this.ObfuscationSpec.TableSpec.ColumnSpecs.SingleOrDefault(c => c.ColumnName.SafeToString().Trim().ToLower() == field.FieldName.SafeToString().Trim().ToLower());
				this.FieldCache.Add(field.FieldIndex, columnSpec);
			}

			if ((object)columnSpec == null)
				return originalFieldValue; // do nothing when no matching column spec

			if ((object)columnSpec.ObfuscationStrategyType == null)
				return originalFieldValue; // do nothing when strategy type not set

			if (!this.ObfuscationStrategyCache.TryGetValue(columnSpec.ObfuscationStrategyType.AssemblyQualifiedName, out obfuscationStrategy))
			{
				obfuscationStrategy = (IObfuscationStrategy)Activator.CreateInstance(columnSpec.ObfuscationStrategyType);

				if ((object)obfuscationStrategy == null)
					throw new InvalidOperationException(string.Format("Unknown obfuscation strategy '{0}' specified for column '{1}'.", columnSpec.ObfuscationStrategyType.AssemblyQualifiedName, field.FieldName));

				this.ObfuscationStrategyCache.Add(columnSpec.ObfuscationStrategyType.AssemblyQualifiedName, obfuscationStrategy);
			}

			obfuscatedFieldValue = obfuscationStrategy.GetObfuscatedValue(this, columnSpec, field, originalFieldValue);

			return obfuscatedFieldValue;
		}

		public /*virtual*/ void Close()
		{
			if (this.Disposed)
				return;

			this.Dispose(true);
			GC.SuppressFinalize(this);

			this.Disposed = true;
		}

		public void Dispose()
		{
			this.Close();
		}

		protected /*virtual*/ void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.SubstitutionCacheRoot.Clear();
				this.FieldCache.Clear();
			}
		}

		private long GetBoundedHash(long? size, object value)
		{
			long? hash;

			hash = GetHash(this.ObfuscationSpec.HashSpec.Multiplier,
				size,
				this.ObfuscationSpec.HashSpec.Seed,
				value.SafeToString());

			if ((object)hash == null)
				throw new InvalidOperationException(string.Format("Oxymoron engine failed to calculate a valid hash for input '{0}'.", value.SafeToString(null, "<null>")));

			return hash.GetValueOrDefault();
		}

		public object GetObfuscatedValue(IField field, object columnValue)
		{
			object value;

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnValue == DBNull.Value)
				columnValue = null;

			if ((this.ObfuscationSpec.EnablePassThru ?? false))
				return columnValue; // pass-thru (saftey switch)

			value = this._GetObfuscatedValue(field, columnValue);

			return value;
		}

		public IEnumerable<IRecord> GetObfuscatedValues(IEnumerable<IRecord> records)
		{
			long fieldIndex;
			long recordIndex;
			string fieldName;
			Type fieldType;
			object originalFieldValue, obfuscatedFieldValue;
			bool isFieldOptional = true;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			recordIndex = 0;
			foreach (IRecord record in records)
			{
				Record obfuscatedRecord = null;

				if ((object)record != null)
				{
					obfuscatedRecord = new Record();

					fieldIndex = 0;
					foreach (KeyValuePair<string, object> item in record)
					{
						IField field; // TODO: should be provided to constructor

						fieldName = item.Key;
						originalFieldValue = record[item.Key];
						fieldType = (originalFieldValue ?? new object()).GetType();

						field = new Field()
								{
									FieldName = fieldName,
									FieldType = fieldType,
									IsFieldOptional = isFieldOptional
								};

						obfuscatedFieldValue = this.GetObfuscatedValue(field, originalFieldValue);
						obfuscatedRecord.Add(fieldName, obfuscatedFieldValue);

						fieldIndex++;
					}
				}

				yield return obfuscatedRecord;
			}
		}

		long IObfuscationContext.GetSignHash(object value)
		{
			long hash;

			hash = this.GetBoundedHash(DEFAULT_HASH_BUCKET_SIZE, value);

			return hash;
		}

		long IObfuscationContext.GetValueHash(long? size, object value)
		{
			long hash;

			hash = this.GetBoundedHash(size ?? DEFAULT_HASH_BUCKET_SIZE, value);

			return hash;
		}

		private object ResolveDictionaryValue(IDictionarySpec dictionarySpec, object surrogateKey)
		{
			if ((object)dictionarySpec == null)
				throw new ArgumentNullException(nameof(dictionarySpec));

			if ((object)surrogateKey == null)
				throw new ArgumentNullException(nameof(surrogateKey));

			if ((object)this.ResolveDictionaryValueCallback == null)
				return null;

			return this.ResolveDictionaryValueCallback(dictionarySpec, surrogateKey);
		}

		bool IObfuscationContext.TryGetSurrogateValue(IDictionarySpec dictionarySpec, object surrogateKey, out object surrogateValue)
		{
			IDictionary<object, object> dictionaryCache;

			if ((object)dictionarySpec == null)
				throw new ArgumentNullException(nameof(dictionarySpec));

			if (!this.ObfuscationSpec.DisableEngineCaches ?? false)
			{
				if (!this.SubstitutionCacheRoot.TryGetValue(dictionarySpec.DictionaryId, out dictionaryCache))
				{
					dictionaryCache = new Dictionary<object, object>();
					this.SubstitutionCacheRoot.Add(dictionarySpec.DictionaryId, dictionaryCache);
				}

				if (!dictionaryCache.TryGetValue(surrogateKey, out surrogateValue))
				{
					if (dictionarySpec.PreloadEnabled)
						throw new InvalidOperationException(string.Format("Cache miss when dictionary preload enabled for dictionary ID '{0}'; current cache slot item count: {1}.", dictionarySpec.DictionaryId, dictionaryCache.Count));

					surrogateValue = this.ResolveDictionaryValue(dictionarySpec, surrogateKey);
					dictionaryCache.Add(surrogateKey, surrogateValue);
				}
			}
			else
			{
				surrogateValue = this.ResolveDictionaryValue(dictionarySpec, surrogateKey);
			}

			return true;
		}

		#endregion
	}
}