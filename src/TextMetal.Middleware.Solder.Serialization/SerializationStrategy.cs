/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace TextMetal.Middleware.Solder.Serialization
{
	public abstract class SerializationStrategy : ISerializationStrategy
	{
		#region Constructors/Destructors

		protected SerializationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified input file.
		/// </summary>
		/// <param name="inputFilePath"> The input file path to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromFile(string inputFilePath, Type targetType)
		{
			object obj;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (Stream stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				obj = this.GetObjectFromStream(stream, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified input file. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="inputFilePath"> The input file path to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromFile<TObject>(string inputFilePath)
		{
			TObject obj;
			Type targetType;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromFile(inputFilePath, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified readable stream.
		/// </summary>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public abstract object GetObjectFromStream(Stream stream, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified readable stream. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromStream<TObject>(Stream stream)
		{
			TObject obj;
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromStream(stream, targetType);

			return obj;
		}

		/// <summary>
		/// Serializes an object to the specified output file.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="outputFilePath"> The output file path to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToFile<TObject>(string outputFilePath, TObject obj)
		{
			Type targetType;

			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToFile(outputFilePath, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified output file.
		/// </summary>
		/// <param name="outputFilePath"> The output file path to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToFile(string outputFilePath, Type targetType, object obj)
		{
			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (Stream stream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				this.SetObjectToStream(stream, targetType, obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToStream<TObject>(Stream stream, TObject obj)
		{
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToStream(stream, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public abstract void SetObjectToStream(Stream stream, Type targetType, object obj);

		#endregion
	}
}