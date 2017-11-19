using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SyncPrem.Controller.Web.Controllers
{
	public class TestController : Microsoft.AspNetCore.Mvc.Controller
	{
		#region Methods/Operators

		[ActionName("index")]
		[HttpGet]
		public IActionResult IndexGet()
		{
			return this.View();
		}

		[ActionName("index")]
		[HttpPost]
		public IActionResult IndexPost()
		{
			IFormFile file = this.Request.Form.Files.SingleOrDefault();

			long len = file.Length;

			return this.Ok();
		}

		#endregion
	}
}