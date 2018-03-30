/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace TextMetal.Middleware.Solder.Serialization
{
	public abstract class BinarySerializationStrategy : SerializationStrategy, IBinarySerializationStrategy
	{
		#region Constructors/Destructors

		protected BinarySerializationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified byte array value.
		/// </summary>
		/// <param name="value"> The byte array value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromBytes(byte[] value, Type targetType)
		{
			object obj;
			BinaryReader binaryReader;

			using (Stream stream = new MemoryStream(value))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = this.GetObjectFromReader(binaryReader, targetType);
					return obj;
				}
			}
		}

		/// <summary>
		/// Deserializes an object from the specified byte array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The byte array value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromBytes<TObject>(byte[] value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromBytes(value, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public abstract object GetObjectFromReader(BinaryReader binaryReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified text reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromReader<TObject>(BinaryReader binaryReader)
		{
			TObject obj;
			Type targetType;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromReader(binaryReader, targetType);

			return obj;
		}

		/// <summary>
		/// Serializes an object to a byte array value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A byte array representation of the object graph. </returns>
		public byte[] SetObjectToBytes(Type targetType, object obj)
		{
			BinaryWriter binaryWriter;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (binaryWriter = new BinaryWriter(memoryStream))
				{
					this.SetObjectToWriter(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		/// <summary>
		/// Serializes an object to a byte array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A byte array representation of the object graph. </returns>
		public byte[] SetObjectToBytes<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return this.SetObjectToBytes(targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="binaryWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public abstract void SetObjectToWriter(BinaryWriter binaryWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified text writer. This is the generic overload.
		/// </summary>
		/// <param name="binaryWriter"> The text writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToWriter<TObject>(BinaryWriter binaryWriter, TObject obj)
		{
			Type targetType;

			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToWriter(binaryWriter, targetType, (object)obj);
		}

		#endregion
	}
}