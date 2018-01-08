using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace MobileOrders.Controllers
{
	[Authorize]
	public class AppController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public AppController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
		}
        public string GetCode()
        {
            return agent.id;
        }
        public IActionResult GetAppFile(string password)
        {
            var period = db.LogApps.Max(s => s.period);
            if (period == null) return StatusCode(404); 
            var app = db.LogApps.FirstOrDefault(s => s.period == period);
            byte[] fileBytes = System.IO.File.ReadAllBytes(app.path);
            return File(fileBytes, app.file);
        }
        public IActionResult GetAppHash(string password)
        {
            var period = db.LogApps.Max(s => s.period);
            if (period == null) return StatusCode(404);
			var app = db.LogApps.FirstOrDefault(s => s.period == period);
            using (FileStream stream = System.IO.File.OpenRead(app.path))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] hash = sha.ComputeHash(stream);
                return Ok(BitConverter.ToString(hash).Replace("-", String.Empty));
            }
        }
        [HttpPost]
        public IActionResult LoadApp(IFormFile fileApp, string password)
        {
			try
			{
				if (fileApp.Length > 0)
				{
					using (var reader = new StreamReader(fileApp.OpenReadStream()))
					{
						var parsedContentDisposition = ContentDispositionHeaderValue.Parse(fileApp.ContentDisposition);
						var fileName = parsedContentDisposition.FileName;
						if (Regex.Match(fileName, @"\.\w{3,4}$", RegexOptions.IgnoreCase).Value != ".apk") return StatusCode(404);
						string path = Path.Combine(@"D:\order\app", fileName);

						using (var stream = new FileStream(path, FileMode.Create))
						{
							fileApp.CopyTo(stream);
						}
						LogApp fileLog = new LogApp { id = 0, file = fileApp.FileName, path = path, period = DateTime.Now };
						db.Entry(fileLog).State = EntityState.Added;
						db.SaveChanges();
					}
				}
			}
            catch (Exception ex)
            {
				return StatusCode(404);
			}
            return Ok("Ok");
        }
    }
}