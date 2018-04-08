using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SyncPrem.StreamingIO.ProxyWrappers.Internal;

namespace SyncPrem.Controller.Web.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : Microsoft.AspNetCore.Mvc.Controller
	{
		#region Methods/Operators

		[HttpPost]
		[DisableRequestSizeLimit]
		public async Task<IActionResult> Post()
		{
			Task task;

			task = Task.Run(() =>
							{
								Stream stream = this.Request.Body;

								using (ProgressWrappedStream inputStream = new ProgressWrappedStream(stream))
								{
									using (ProgressWrappedStream outputStream = new ProgressWrappedStream(System.IO.File.Create(Path.Combine("d:\\", $"api-post-{Guid.NewGuid().ToString("N")}.json"))))
									{
										inputStream.CopyTo(outputStream);
									}
								}
							});

			await task;

			if (task.IsCompletedSuccessfully)
				return this.Ok();
			else
				return this.StatusCode(500);
		}

		#endregion
	}
}