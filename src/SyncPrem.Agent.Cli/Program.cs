using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrem.Agent.Cli
{
	internal class Program
	{
		#region Fields/Constants

		private static readonly IEnumerable<int> inf = InfiniteSeries();
		private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		#endregion

		#region Properties/Indexers/Events

		private static CancellationTokenSource CancellationTokenSource
		{
			get
			{
				return cancellationTokenSource;
			}
		}

		#endregion

		#region Methods/Operators

		private static async Task _Main(string[] args)
		{
			CancellationToken cancellationToken;

			cancellationToken = CancellationTokenSource.Token;

			Console.CancelKeyPress += ConsoleOnCancelKeyPress;
			IObservable<double> obs = GetObservable();

			/*Console.WriteLine("before");
			foreach (var d in obs.ToAsyncEnumerable().ToEnumerable())
			{
				Console.WriteLine(d);
			}
			Console.WriteLine("after");*/

			obs.Subscribe(OnNext, OnCompleted, cancellationToken);
			obs.Subscribe(OnNext, OnCompleted, cancellationToken);
			var canceled = await Maybe(Task.Delay(-1, cancellationToken));

			Console.WriteLine(canceled);
		}

		private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
		{
			if (consoleCancelEventArgs.SpecialKey == ConsoleSpecialKey.ControlC)
			{
				CancellationTokenSource.Cancel();
				consoleCancelEventArgs.Cancel = true;
			}
		}

		private static async Task ExecPipeAsync()
		{
			IFoobar foobar = await SourceAsync();
			await SinkAsync(foobar);
		}

		private static IObservable<double> GetObservable()
		{
			return Observable.Create<double>(
				async _ =>
				{
					for (int i = 0; i < 10; i++)
					{
						await Task.Delay(250);
						Console.WriteLine(1.0 * i);
						_.OnNext(1.0 * i);
					}

					_.OnCompleted();
				});
		}

		private static IEnumerable<double> GetYieldReturnEnumerable()
		{
			Print(Environment.CurrentManagedThreadId.ToString());
			for (int i = 0; i < 10; i++)
			{
				Print(".");
				yield return i;
			}

			Print(Environment.NewLine);
		}

		public static IEnumerable<int> InfiniteSeries()
		{
			int result = 0;
			while (true)
			{
				yield return result;
				result++;
			}
		}

		private static void Main(string[] args)
		{
			Action x = (() => Console.WriteLine("count {1}@{0}", Environment.CurrentManagedThreadId, inf.Take(10).Count()));
			//x();
			Task.Run(x).Wait();
		}

		private static async Task<bool> Maybe(Task task)
		{
			try
			{
				await task;
			}
			catch (TaskCanceledException ex)
			{
				return true;
			}

			return false;
		}

		private static void OnCompleted()
		{
			//CancellationTokenSource.Cancel();
		}

		private static void OnNext(double d)
		{
			Console.WriteLine("next: {0}", d);
		}

		private static void Print(string message)
		{
			Console.Write(message);
		}

		private static async Task Run(CancellationToken cancellationToken)
		{
			List<Task> tasks = new List<Task>();
			Print("Hello world");

			while (!cancellationToken.IsCancellationRequested)
			{
				Console.WriteLine("poll() begin");
				for (int i = 0; i < 10; i++)
				{
					Task task;
					task = ExecPipeAsync();
					tasks.Add(task);
				}

				Console.WriteLine("poll() await...");
				await Task.WhenAll(tasks);

				Console.WriteLine("poll() delay...");
				await Task.Delay(TimeSpan.FromMilliseconds(5000), cancellationToken);
				Console.WriteLine("poll() end");
			}

			Print("Goodbye world");
		}

		private static Task SinkAsync(IFoobar foobar)
		{
			foobar.Doubles.ToArray();
			return Task.CompletedTask;
		}

		private static async Task<IFoobar> SourceAsync()
		{
			var doubles = GetObservable();
			await Task.Delay(1000);
			return new Foobar(doubles);
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		public class Foobar : IFoobar
		{
			#region Constructors/Destructors

			public Foobar(IObservable<double> doubles)
			{
				this.doubles = doubles;
			}

			#endregion

			#region Fields/Constants

			private readonly IObservable<double> doubles;

			#endregion

			#region Properties/Indexers/Events

			public IObservable<double> Doubles
			{
				get
				{
					return this.doubles;
				}
			}

			#endregion
		}

		public interface IFoobar
		{
			#region Properties/Indexers/Events

			IObservable<double> Doubles
			{
				get;
			}

			#endregion
		}

		#endregion
	}
}