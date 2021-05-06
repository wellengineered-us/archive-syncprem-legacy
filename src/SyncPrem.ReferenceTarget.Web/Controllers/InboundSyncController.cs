using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SyncPrem.StreamingIO.ProxyWrappers.Internal;

namespace SyncPrem.ReferenceTarget.Web.Controllers
{
	[Route("api/[controller]")]
	public class InboundSyncController : Controller
	{
		#region Methods/Operators

		[HttpPost]
		[DisableRequestSizeLimit]
		public async Task<IActionResult> Post()
		{
			Stream stream = this.Request.Body;
			string file = null;
			
			using (ProgressWrappedStream inputStream = new ProgressWrappedStream(stream))
			{
				using (ProgressWrappedStream outputStream = /*new ProgressWrappedStream(Stream.Null)*/
					new ProgressWrappedStream(System.IO.File.Create(Path.Combine("d:\\", file = $"api-post-{Guid.NewGuid().ToString("N")}.json")))
				)
				{
					await inputStream.CopyToAsync(outputStream);
				}
			}
			
			Console.WriteLine(file);

			return this.Created(file, "");
		}

		[HttpGet]
		[HttpHead]
		[DisableRequestSizeLimit]
		public async Task<IActionResult> Get()
		{
			return this.Content("{}");
		}
		
		#endregion
	}
}