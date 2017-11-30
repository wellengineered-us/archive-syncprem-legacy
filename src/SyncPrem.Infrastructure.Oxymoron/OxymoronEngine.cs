/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Oxymoron.Configuration;
using SyncPrem.Infrastructure.Oxymoron.Strategies;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Infrastructure.Oxymoron
{
	public sealed class OxymoronEngine : IOxymoronEngine, IObfuscationContext
	{
		#region Constructors/Destructors

		public OxymoronEngine(ResolveDictionaryValueDelegate resolveDictionaryValueCallback, ObfuscationConfiguration obfuscationConfiguration)
		{
			if ((object)resolveDictionaryValueCallback == null)
				throw new ArgumentNullException(nameof(resolveDictionaryValueCallback));

			if ((object)obfuscationConfiguration == null)
				throw new ArgumentNullException(nameof(obfuscationConfiguration));

			this.resolveDictionaryValueCallback = resolveDictionaryValueCallback;
			this.obfuscationConfiguration = obfuscationConfiguration;

			//OnlyWhen._DEBUG_ThenPrint(string.Format("SECURITY_CRITICAL_OPTION: {0}={1}", nameof(obfuscationConfiguration.EnablePassThru), obfuscationConfiguration.EnablePassThru ?? false));
			EnsureValidConfigurationOnce(this.ObfuscationConfiguration);
		}

		#endregion

		#region Fields/Constants

		private const long DEFAULT_HASH_BUCKET_SIZE = long.MaxValue;
		private const bool SUBSTITUTION_CACHE_ENABLED = true;
		private readonly IDictionary<int, ColumnConfiguration> fieldCache = new Dictionary<int, ColumnConfiguration>();
		private readonly ObfuscationConfiguration obfuscationConfiguration;
		private readonly IDictionary<string, IObfuscationStrategy> obfuscationStrategyCache = new Dictionary<string, IObfuscationStrategy>();
		private readonly ResolveDictionaryValueDelegate resolveDictionaryValueCallback;
		private readonly IDictionary<string, IDictionary<object, object>> substitutionCacheRoot = new Dictionary<string, IDictionary<object, object>>();
		private bool disposed;

		#endregion

		#region Properties/Indexers/Events

		private IDictionary<int, ColumnConfiguration> FieldCache
		{
			get
			{
				return this.fieldCache;
			}
		}

		public ObfuscationConfiguration ObfuscationConfiguration
		{
			get
			{
				return this.obfuscationConfiguration;
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

		/// <summary>
		/// NOTE: Calling this more than once kills performance.
		/// </summary>
		/// <param name="configurationObject"> </param>
		private static void EnsureValidConfigurationOnce(ConfigurationObject configurationObject)
		{
			IEnumerable<Message> messages;

			if ((object)configurationObject == null)
				throw new ArgumentNullException(nameof(configurationObject));

			messages = configurationObject.Validate();

			if (messages.Any())
				throw new InvalidOperationException(string.Format("Obfuscation configuration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));
		}

		public static TConfiguration FromJsonFile<TConfiguration>(string jsonFilePath)
			where TConfiguration : class, IConfigurationObject, new()
		{
			TConfiguration configuration;

			configuration = JsonSerializationStrategy.Instance.GetObjectFromFile<TConfiguration>(jsonFilePath);

			return configuration;
		}

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

		public static void ToJsonFile<TConfiguration>(TConfiguration configuration, string jsonFilePath)
			where TConfiguration : class, IConfigurationObject, new()
		{
			JsonSerializationStrategy.Instance.SetObjectToFile<TConfiguration>(jsonFilePath, configuration);
		}

		private object _GetObfuscatedValue(IField field, object originalFieldValue)
		{
			IObfuscationStrategy obfuscationStrategy;
			ColumnConfiguration columnConfiguration;
			object obfuscatedFieldValue;

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			// KILLS performance - not sure why
			//EnsureValidConfigurationOnce(this.TableConfiguration);
			//columnConfiguration = this.TableConfiguration.ColumnConfigurations.SingleOrDefault(c => c.ColumnName.SafeToString().Trim().ToLower() == columnName.SafeToString().Trim().ToLower());

			if (!this.FieldCache.TryGetValue(field.FieldIndex, out columnConfiguration))
			{
				columnConfiguration = this.ObfuscationConfiguration.TableConfiguration.ColumnConfigurations.SingleOrDefault(c => c.ColumnName.SafeToString().Trim().ToLower() == field.FieldName.SafeToString().Trim().ToLower());
				//columnConfiguration = this.TableConfiguration.ColumnConfigurations.SingleOrDefault(c => c.ColumnName == columnName);
				this.FieldCache.Add(field.FieldIndex, columnConfiguration);
			}

			if ((object)columnConfiguration == null)
				return originalFieldValue; // do nothing when no matching column configuration

			if (!this.ObfuscationStrategyCache.TryGetValue(columnConfiguration.ObfuscationStrategyAqtn, out obfuscationStrategy))
			{
				obfuscationStrategy = columnConfiguration.GetObfuscationStrategyInstance();

				if ((object)obfuscationStrategy == null)
					throw new InvalidOperationException(string.Format("Unknown obfuscation strategy '{0}' specified for column '{1}'.", columnConfiguration.ObfuscationStrategyAqtn, field.FieldName));

				this.ObfuscationStrategyCache.Add(columnConfiguration.ObfuscationStrategyAqtn, obfuscationStrategy);
			}

			obfuscatedFieldValue = obfuscationStrategy.GetObfuscatedValue(this, columnConfiguration, field, originalFieldValue);

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

			hash = GetHash(this.ObfuscationConfiguration.HashConfiguration.Multiplier,
				size,
				this.ObfuscationConfiguration.HashConfiguration.Seed,
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

			if ((this.ObfuscationConfiguration.EnablePassThru ?? false))
				return columnValue; // pass-thru

			value = this._GetObfuscatedValue(field, columnValue);

			return value;
		}

		public IEnumerable<IRecord> GetObfuscatedValues(IEnumerable<IRecord> records)
		{
			int fieldIndex;
			string fieldName;
			Type fieldType;
			object originalFieldValue, obfuscatedFieldValue;
			bool isFieldOptional = true;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			foreach (IRecord record in records)
			{
				Record obfuscatedRecord = null;

				if ((object)record != null)
				{
					obfuscatedRecord = new Record(record.RecordIndex) { ContextData = record };

					fieldIndex = 0;
					foreach (KeyValuePair<string, object> item in record)
					{
						IField field;

						fieldName = item.Key;
						originalFieldValue = record[item.Key];
						fieldType = (originalFieldValue ?? new object()).GetType();

						field = new Field(fieldIndex++)
								{
									FieldName = fieldName,
									FieldType = fieldType,
									IsFieldOptional = isFieldOptional,
									ContextData = item
								};

						obfuscatedFieldValue = this.GetObfuscatedValue(field, originalFieldValue);
						obfuscatedRecord.Add(fieldName, obfuscatedFieldValue);
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

		private object ResolveDictionaryValue(DictionaryConfiguration dictionaryConfiguration, object surrogateKey)
		{
			if ((object)dictionaryConfiguration == null)
				throw new ArgumentNullException(nameof(dictionaryConfiguration));

			if ((object)surrogateKey == null)
				throw new ArgumentNullException(nameof(surrogateKey));

			if ((object)this.ResolveDictionaryValueCallback == null)
				return null;

			return this.ResolveDictionaryValueCallback(dictionaryConfiguration, surrogateKey);
		}

		bool IObfuscationContext.TryGetSurrogateValue(DictionaryConfiguration dictionaryConfiguration, object surrogateKey, out object surrogateValue)
		{
			IDictionary<object, object> dictionaryCache;

			if ((object)dictionaryConfiguration == null)
				throw new ArgumentNullException(nameof(dictionaryConfiguration));

			if (!this.ObfuscationConfiguration.DisableEngineCaches ?? false)
			{
				if (!this.SubstitutionCacheRoot.TryGetValue(dictionaryConfiguration.DictionaryId, out dictionaryCache))
				{
					dictionaryCache = new Dictionary<object, object>();
					this.SubstitutionCacheRoot.Add(dictionaryConfiguration.DictionaryId, dictionaryCache);
				}

				if (!dictionaryCache.TryGetValue(surrogateKey, out surrogateValue))
				{
					if (dictionaryConfiguration.PreloadEnabled)
						throw new InvalidOperationException(string.Format("Cache miss when dictionary preload enabled for dictionary ID '{0}'; current cache slot item count: {1}.", dictionaryConfiguration.DictionaryId, dictionaryCache.Count));

					surrogateValue = this.ResolveDictionaryValue(dictionaryConfiguration, surrogateKey);
					dictionaryCache.Add(surrogateKey, surrogateValue);
				}
			}
			else
			{
				surrogateValue = this.ResolveDictionaryValue(dictionaryConfiguration, surrogateKey);
			}

			return true;
		}

		#endregion
	}
}