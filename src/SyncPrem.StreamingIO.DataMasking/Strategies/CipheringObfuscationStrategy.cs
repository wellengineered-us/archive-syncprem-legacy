/*
	Copyright ?2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.DataMasking.Strategies
{
	/// <summary>
	/// Returns an alternate value that is a binary encryption of the original value.
	/// DATA TYPE: string
	/// </summary>
	public sealed class CipheringObfuscationStrategy : ObfuscationStrategy<CipheringObfuscationStrategy.Spec>
	{
		#region Constructors/Destructors

		public CipheringObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		private static object GetCipher(string sharedSecret, object value)
		{
			const string INIT_VECTOR = "0123456701234567";
			const int KEY_SIZE = 256;

			byte[] initVectorBytes;
			byte[] plainTextBytes;
			ICryptoTransform encryptor;
			byte[] keyBytes;
			byte[] cipherTextBytes;
			Type valueType;
			string _value;

			if ((object)sharedSecret == null)
				throw new ArgumentNullException(nameof(sharedSecret));

			if ((object)value == null)
				return null;

			valueType = value.GetType();

			if (valueType != typeof(String))
				return null;

			_value = (String)value;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(_value))
				return _value;

			_value = _value.Trim();

			initVectorBytes = Encoding.UTF8.GetBytes(INIT_VECTOR);
			plainTextBytes = Encoding.UTF8.GetBytes(_value);

			using (DeriveBytes password = new Rfc2898DeriveBytes(sharedSecret, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }))
			{
				keyBytes = password.GetBytes(KEY_SIZE / 8);
			}

			// Rijndael not yet supported in .NET Core
			using (Aes symmetricKey = Aes.Create())
			{
				symmetricKey.Mode = CipherMode.CBC;
				encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
					{
						cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
						cryptoStream.FlushFinalBlock();
						cipherTextBytes = memoryStream.ToArray();
					}
				}
			}

			return Encoding.UTF8.GetString(cipherTextBytes);
		}

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<Spec> columnSpec, IField field, object columnValue)
		{
			long signHash, valueHash;
			object value;
			string sharedSecret;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			signHash = obfuscationContext.GetSignHash(columnValue);
			valueHash = obfuscationContext.GetValueHash(null, columnValue);
			sharedSecret = ((valueHash <= 0 ? 1 : valueHash) * (signHash % 2 == 0 ? -1 : 1)).ToString("X");

			value = GetCipher(sharedSecret, columnValue);

			return value;
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		public sealed class Spec : IObfuscationStrategySpec
		{
			#region Constructors/Destructors

			public Spec()
			{
			}

			#endregion
		}

		#endregion
	}
}