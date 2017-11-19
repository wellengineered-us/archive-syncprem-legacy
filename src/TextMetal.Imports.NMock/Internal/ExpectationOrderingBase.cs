#region Using

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NMock.Monitoring;

#endregion

namespace NMock.Internal
{
	internal abstract class ExpectationListBase : List<IExpectation>, IExpectationList
	{
		internal const string NO_ACTIVE_EXPECTATIONS = "All expectations have been met.  Either no expectations have been defined or all defined expectations have been fulfilled.  You may try increasing the expectation count of one of your expectations, or decreasing the number of calls to the member defined in the Expectation Exception.";

		internal const string NO_EXPECTATIONS = "No expectations have been defined.";

		/// <summary>
		/// Stores the calling depth for the document writer output.
		/// </summary>
		protected int depth;

		protected string prompt;

		protected ExpectationListBase(ExpectationListBase parent)
		{
			Parent = parent;
		}

		public int Depth
		{
			get { return depth; }
		}

		/// <summary>
		/// Gets or sets the expectation list that contains this ordering
		/// </summary>
		private ExpectationListBase Parent { get; set; }

		/// <summary>
		/// Gets the root <see cref="IExpectationList"/> for the <see cref="MockFactory"/>
		/// </summary>
		internal IExpectationList Root
		{
			get
			{
				ExpectationListBase current = this;
				while (current.Parent != null)
					current = current.Parent;

				return current;
			}
		}

		public void AddExpectation(IExpectation expectation)
		{
			Add(expectation);
		}

		public void RemoveExpectation(IExpectation expectation)
		{
			var removed = Remove(expectation);

			if (!removed)
			{
				foreach (var list in this.OfType<IExpectationList>())
				{
					list.RemoveExpectation(expectation);
				}
			}
		}

		public abstract bool ContainsOrderedExpectationFor(Invocation invocation);
		public abstract bool IsActive { get; }
		public abstract bool IsValid { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has been met.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance has been met; otherwise, <c>false</c>.
		/// </value>
		public bool HasBeenMet
		{
			get
			{
				return this.All(e => e.HasBeenMet);
			}
		}

		void IExpectation.DescribeActiveExpectationsTo(TextWriter writer)
		{
			Indent(writer, depth);
			writer.WriteLine(prompt);

			bool expectationWritten = false;

			foreach (IExpectation expectation in this)
			{
				if (expectation.IsActive)
				{
					if (!(expectation is IExpectationList))
						Indent(writer, depth + 1);

					expectation.DescribeActiveExpectationsTo(writer);

					expectationWritten = true;
				}
			}

			if (depth != 0)
			{
				Indent(writer, depth);
				writer.WriteLine("}");
			}

			if (!expectationWritten)
			{
				Indent(writer, depth);
				writer.Write(NO_ACTIVE_EXPECTATIONS);
			}
		}

		public void DescribeUnmetExpectationsTo(TextWriter writer)
		{
			Indent(writer, depth);
			writer.WriteLine(prompt);

			bool expectationWritten = false;

			foreach (IExpectation expectation in this)
			{
				if (!expectation.HasBeenMet)
				{
					if (!(expectation is IExpectationList))
						Indent(writer, depth + 1);

					expectation.DescribeUnmetExpectationsTo(writer);

					expectationWritten = true;
				}
			}

			if (depth != 0)
			{
				Indent(writer, depth);
				writer.WriteLine("}");
			}

			if (!expectationWritten)
			{
				Indent(writer, depth);
				writer.Write(NO_ACTIVE_EXPECTATIONS);
			}
		}

		public void DescribeTo(TextWriter writer)
		{
			if (Count == 0 && Depth != 0)
				return;

			Indent(writer, depth);
			writer.WriteLine(prompt);

			var expectationsWritten = false;
			foreach (var expectation in this)
			{
				if (!(expectation is IExpectationList))
					Indent(writer, depth + 1);

				expectation.DescribeTo(writer);
				expectationsWritten = true;
			}

			if (!expectationsWritten)
			{
				Indent(writer, depth + 1);
				writer.WriteLine(NO_EXPECTATIONS);
			}

			if (depth != 0)
			{
				Indent(writer, depth);
				writer.WriteLine("}");
			}
		}

		string IExpectation.ValidationErrors()
		{
			return "An expectation list does not have validation errors.";
		}

		void IVerifyableExpectation.Assert()
		{
			ForEach(_ => _.Assert());
		}


		/// <summary>
		/// Adds all expectations to <paramref name="result"/> that are associated to <paramref name="mock"/>.
		/// </summary>
		/// <param name="mock">The mock for which expectations are queried.</param>
		/// <param name="result">The result to add matching expectations to.</param>
		public void QueryExpectationsBelongingTo(IMockObject mock, IList<IExpectation> result)
		{
			ForEach(expectation => expectation.QueryExpectationsBelongingTo(mock, result));
		}

		public abstract bool Perform(Invocation invocation);
		public abstract bool Matches(Invocation invocation);
		public abstract bool MatchesIgnoringIsActive(Invocation invocation);

		/*
		private void Validate()
		{
			var expectation = this.FirstOrDefault(_ => !_.IsValid);

			if(expectation != null)
			{
				throw new IncompleteExpectationException(expectation);
			}
		}
		*/

		private void Indent(TextWriter writer, int n)
		{
			for (var i = 0; i < n; i++)
			{
				writer.Write("  ");
			}
		}
	}
}