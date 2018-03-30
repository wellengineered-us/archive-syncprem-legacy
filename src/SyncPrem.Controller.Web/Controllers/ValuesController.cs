using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.AspNetCore.Mvc;

using SyncPrem.StreamingIO.ProxyWrappers.Internal;

namespace SyncPrem.Controller.Web.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : Microsoft.AspNetCore.Mvc.Controller
	{
		#region Methods/Operators

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		// GET api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		//[HttpPost]
		//public void Post([FromBody] string value)
		//{
		//}

		// POST api/values
		[HttpPost]
		[DisableRequestSizeLimit]
		public IActionResult Post()
		{
			Stream stream = this.Request.Body;

			using (ProgressWrappedStream inputStream = new ProgressWrappedStream(stream))
			{
				using (ProgressWrappedStream outputStream = new ProgressWrappedStream(System.IO.File.Create(Path.Combine("d:\\", "api-post.json"))))
				{
					inputStream.CopyTo(outputStream);
				}
			}

			return this.Ok("test");
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		#endregion
	}
}