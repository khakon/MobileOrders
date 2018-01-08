using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;
using Microsoft.AspNetCore.Authorization;

namespace MobileOrders.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
	[Authorize]
	public class CustomersController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public CustomersController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		public IActionResult Get()
		{
			try
			{
				agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
				var model = db.Customers.Where(m => m.idAgent.Trim() == agent.id).OrderBy(s => s.Kontr);
				return Ok(model);
			}
			catch(Exception ex)
			{
				return Ok(ex.Message);
			}

		}
	}
}