/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SyncPrem.Infrastructure.Data.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Infrastructure.Textual.Delimited
{
	public class DelimitedTextReader : TextualReader
	{
		#region Constructors/Destructors

		public DelimitedTextReader(TextReader baseTextReader, IDelimitedTextSpec delimitedTextSpec)
			: base(baseTextReader)
		{
			if ((object)delimitedTextSpec == null)
				throw new ArgumentNullException(nameof(delimitedTextSpec));

			this.delimitedTextSpec = delimitedTextSpec;

			this.ResetParserState();
		}

		#endregion

		#region Fields/Constants

		private readonly IDelimitedTextSpec delimitedTextSpec;
		private readonly _ParserState parserState = new _ParserState();

		#endregion

		#region Properties/Indexers/Events

		public IDelimitedTextSpec DelimitedTextSpec
		{
			get
			{
				return this.delimitedTextSpec;
			}
		}

		private _ParserState ParserState
		{
			get
			{
				return this.parserState;
			}
		}

		#endregion

		#region Methods/Operators

		private static bool LookBehindFixup(StringBuilder targetStringBuilder, string targetValue)
		{
			if ((object)targetStringBuilder == null)
				throw new ArgumentNullException(nameof(targetStringBuilder));

			if ((object)targetValue == null)
				throw new ArgumentNullException(nameof(targetValue));

			if (string.IsNullOrEmpty(targetValue))
				throw new ArgumentOutOfRangeException(nameof(targetValue));

			// look-behind CHECK
			if (targetStringBuilder.Length > 0 &&
				targetValue.Length > 0 &&
				targetStringBuilder.Length >= targetValue.Length)
			{
				int sb_length;
				int rd_length;
				int matches;

				sb_length = targetStringBuilder.Length;
				rd_length = targetValue.Length;
				matches = 0;

				for (int i = 0; i < rd_length; i++)
				{
					if (targetStringBuilder[sb_length - rd_length + i] != targetValue[i])
						return false; // look-behind NO MATCH...

					matches++;
				}

				if (matches != rd_length)
					throw new InvalidOperationException(string.Format("Something went sideways."));

				targetStringBuilder.Remove(sb_length - rd_length, rd_length);

				// look-behind MATCHED: stop
				return true;
			}

			return false; // not enough buffer space to care
		}

		private bool ParserStateMachine()
		{
			bool succeeded;
			string tempStringValue;
			bool matchedRecordDelimiter, matchedFieldDelimiter;

			// now determine what to do based on parser state
			matchedRecordDelimiter = !this.ParserState.isQuotedValue &&
									!string.IsNullOrEmpty(this.DelimitedTextSpec.RecordDelimiter) &&
									LookBehindFixup(this.ParserState.transientStringBuilder, this.DelimitedTextSpec.RecordDelimiter);

			if (!matchedRecordDelimiter)
			{
				matchedFieldDelimiter = !this.ParserState.isQuotedValue &&
										!string.IsNullOrEmpty(this.DelimitedTextSpec.FieldDelimiter) &&
										LookBehindFixup(this.ParserState.transientStringBuilder, this.DelimitedTextSpec.FieldDelimiter);
			}
			else
				matchedFieldDelimiter = false;

			if (matchedRecordDelimiter || matchedFieldDelimiter || this.ParserState.isEOF)
			{
				// RECORD_DELIMITER | FIELD_DELIMITER | EOF

				// get string and clear for exit
				tempStringValue = this.ParserState.transientStringBuilder.ToString();
				this.ParserState.transientStringBuilder.Clear();

				// common logic to store value of field in record
				if (this.ParserState.isHeaderRecord)
				{
					// stash header if FRIS enabled and zeroth record
					this.ParserState.record.Add(tempStringValue, this.ParserState.fieldIndex.ToString("0000"));
				}
				else
				{
					IField header;
					Type fieldType;
					object fieldValue;

					// check header array and field index validity
					if ((object)this.DelimitedTextSpec.HeaderSpecs == null ||
						this.ParserState.fieldIndex >= this.DelimitedTextSpec.HeaderSpecs.Count)
						throw new InvalidOperationException(string.Format("Delimited text reader parse state failure: field index '{0}' exceeded known field indices '{1}' at character index '{2}'.", this.ParserState.fieldIndex, (object)this.DelimitedTextSpec.HeaderSpecs != null ? (this.DelimitedTextSpec.HeaderSpecs.Count - 1) : (int?)null, this.ParserState.characterIndex));

					header = this.DelimitedTextSpec.HeaderSpecs[this.ParserState.fieldIndex];

					fieldType = header.FieldType ?? typeof(string);

					succeeded = true;
					if (string.IsNullOrWhiteSpace(tempStringValue))
						fieldValue = SolderFascadeAccessor.DataTypeFascade.DefaultValue(fieldType);
					else
						succeeded = SolderFascadeAccessor.DataTypeFascade.TryParse(fieldType, tempStringValue, out fieldValue);

					if (!succeeded)
						throw new InvalidOperationException(string.Format("Delimited text reader parse state failure: field string value '{0}' could not be parsed into a valid '{1}'.", tempStringValue, fieldType.FullName));

					// lookup header name (key) by index and commit value to record
					this.ParserState.record.Add(header.FieldName, fieldValue);
				}

				// handle blank lines (we assume that any records with valid record delimiter is OK)
				if (string.IsNullOrEmpty(tempStringValue) && this.ParserState.record.Keys.Count == 1)
					this.ParserState.record = null;

				// now what to do?
				if (this.ParserState.isEOF)
					return true;
				else if (matchedRecordDelimiter)
				{
					// advance record index
					this.ParserState.recordIndex++;

					// reset field index
					this.ParserState.fieldIndex = 0;

					// reset value index
					this.ParserState.valueIndex = 0;

					return true;
				}
				else if (matchedFieldDelimiter)
				{
					// advance field index
					this.ParserState.fieldIndex++;

					// reset value index
					this.ParserState.valueIndex = 0;
				}
			}
			else if (!this.ParserState.isEOF &&
					!this.ParserState.isQuotedValue &&
					!string.IsNullOrEmpty(this.DelimitedTextSpec.QuoteValue) &&
					LookBehindFixup(this.ParserState.transientStringBuilder, this.DelimitedTextSpec.QuoteValue))
			{
				// BEGIN::QUOTE_VALUE
				this.ParserState.isQuotedValue = true;
			}
			//else if (!this.ParserState.isEOF &&
			//	this.ParserState.isQuotedValue &&
			//	!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextSpec.QuoteValue) &&
			//	LookBehindFixup(this.ParserState.transientStringBuilder, this.DelimitedTextSpec.QuoteValue) &&
			//	this.ParserState.peekNextCharacter.ToString() == this.DelimitedTextSpec.QuoteValue)
			//{
			//	// unescape::QUOTE_VALUE
			//	this.ParserState.transientStringBuilder.Append("'");
			//}
			else if (!this.ParserState.isEOF &&
					this.ParserState.isQuotedValue &&
					!string.IsNullOrEmpty(this.DelimitedTextSpec.QuoteValue) &&
					LookBehindFixup(this.ParserState.transientStringBuilder, this.DelimitedTextSpec.QuoteValue))
			{
				// END::QUOTE_VALUE
				this.ParserState.isQuotedValue = false;
			}
			else if (!this.ParserState.isEOF)
			{
				// {field_data}

				// advance content index
				this.ParserState.contentIndex++;

				// advance value index
				this.ParserState.valueIndex++;
			}
			else
			{
				// {unknown_parser_state_error}
				throw new InvalidOperationException(string.Format("Unknown parser state error at character index '{0}'.", this.ParserState.characterIndex));
			}

			return false;
		}

		public override IEnumerable<IField> ReadHeaders()
		{
			IRecord headerRecord;

			if (this.ParserState.recordIndex == 0 &&
				(this.DelimitedTextSpec.FirstRecordIsHeader ?? false))
			{
				IEnumerable<IRecord> y = this.ResumableParserMainLoop(true);

				headerRecord = y.SingleOrDefault(); // force a single enumeration - yield return is a brain fyck
			}

			return this.DelimitedTextSpec.HeaderSpecs;
		}

		public override IEnumerable<IRecord> ReadRecords()
		{
			return this.ResumableParserMainLoop(false);
		}

		public override IEnumerable<IResult> ReadResults()
		{
			IEnumerable<IRecord> records;
			IEnumerable<IResult> results;

			records = this.ReadRecords();
			results = records.ToResults();

			return results;
		}

		private void ResetParserState()
		{
			this.ParserState.record = new Record(0);
			this.ParserState.transientStringBuilder = new StringBuilder();
			this.ParserState.readCurrentCharacter = '\0';
			this.ParserState.peekNextCharacter = '\0';
			this.ParserState.characterIndex = 0;
			this.ParserState.contentIndex = 0;
			this.ParserState.recordIndex = 0;
			this.ParserState.fieldIndex = 0;
			this.ParserState.valueIndex = 0;
			this.ParserState.isQuotedValue = false;
			this.ParserState.isEOF = false;

			this.DelimitedTextSpec.AssertValid();
		}

		private IEnumerable<IRecord> ResumableParserMainLoop(bool once)
		{
			int __value;
			char ch;

			// main loop - character stream
			while (!this.ParserState.isEOF)
			{
				// read the next byte
				__value = this.BaseTextReader.Read();
				ch = (char)__value;

				// check for -1 (EOF)
				if (__value == -1)
				{
					this.ParserState.isEOF = true; // set terminal state

					// sanity check - should never end with an open quote value
					if (this.ParserState.isQuotedValue)
						throw new InvalidOperationException(string.Format("Delimited text reader parse state failure: end of file encountered while reading open quoted value."));
				}
				else
				{
					// append character to temp buffer
					this.ParserState.readCurrentCharacter = ch;
					this.ParserState.transientStringBuilder.Append(ch);

					// advance character index
					this.ParserState.characterIndex++;
				}

				// eval on every loop
				this.ParserState.isHeaderRecord = this.ParserState.recordIndex == 0 && (this.DelimitedTextSpec.FirstRecordIsHeader ?? false);

				// peek the next byte
				__value = this.BaseTextReader.Peek();
				ch = (char)__value;
				this.ParserState.peekNextCharacter = ch;

				if (this.ParserStateMachine())
				{
					if ((object)this.ParserState.record != null)
					{
						// TODO: refactor this up to the ReadHeaders() method
						if (this.ParserState.isHeaderRecord)
						{
							string[] headerNames;
							Field header;

							headerNames = this.ParserState.record.Keys.ToArray();

							// stash parsed header names into header specs member
							if ((object)this.DelimitedTextSpec.HeaderSpecs != null &&
								headerNames.Length == this.DelimitedTextSpec.HeaderSpecs.Count)
							{
								if ((object)headerNames != null)
								{
									for (int headerIndex = 0; headerIndex < headerNames.Length; headerIndex++)
									{
										header = this.DelimitedTextSpec.HeaderSpecs[headerIndex];

										if (!string.IsNullOrWhiteSpace(header.FieldName) &&
											header.FieldName.ToLower() != headerNames[headerIndex].ToLower())
											throw new InvalidOperationException(string.Format("Header name mismatch: '{0}' <> '{1}'.", header.FieldName, headerNames[headerIndex]));

										header.FieldName = headerNames[headerIndex];
									}
								}
							}
							else
							{
								// reset header specs because they do not match in length
								this.DelimitedTextSpec.HeaderSpecs.Clear();

								if ((object)headerNames != null)
								{
									foreach (string headerName in headerNames)
									{
										this.DelimitedTextSpec.HeaderSpecs.Add(new Field()
																				{
																					FieldName = headerName,
																					FieldType = typeof(string)
																				});
									}
								}
							}
						}
						else
						{
							// aint this some shhhhhhhh!t?
							yield return this.ParserState.record;
						}
					}

					this.ParserState.record = new Record(this.ParserState.recordIndex);

					if (once) // state-based resumption of loop ;)
						break;
				}
			}
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		private sealed class _ParserState
		{
			#region Fields/Constants

			public long characterIndex;
			public long contentIndex;
			public IRecord record;
			public int fieldIndex;
			public bool isEOF;
			public bool isHeaderRecord;
			public bool isQuotedValue;
			public char peekNextCharacter;
			public char readCurrentCharacter;
			public int recordIndex;
			public StringBuilder transientStringBuilder;
			public int valueIndex;

			#endregion
		}

		#endregion
	}
}