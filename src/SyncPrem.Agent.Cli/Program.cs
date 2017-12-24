using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.StreamingIO.ProxyWrappers.Internal;

namespace SyncPrem.Agent.Cli
{
	internal class Program
	{
		#region Methods/Operators

		private static void DoWork()
		{
			using (HttpClient httpClient = new HttpClient())
			{
				HttpContent httpContent;

				using (Stream stream = new ProgressWrappedStream(File.OpenRead(Path.Combine("d:\\", "t8.shakespeare.txt"))))
				{
					httpContent = new StreamContent(stream);

					using (Task<HttpResponseMessage> streamTask = httpClient.PostAsync("http://localhost:57087/api/values", httpContent))
					{
						streamTask.Wait();
						HttpResponseMessage result = streamTask.Result;
						Console.WriteLine(result.ToString());
					}
				}
			}
		}

		private static void Main(string[] args)
		{
			Thread.Sleep(2000);
			Console.ForegroundColor = ConsoleColor.Yellow;
			DoWork();

			Console.WriteLine("the end");
		}

		#endregion
	}
}