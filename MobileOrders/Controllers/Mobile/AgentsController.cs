using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;
using Microsoft.AspNetCore.Cors;

namespace MobileOrders.Controllers.Mobile
{

	[Produces("application/json")]
    [Route("mobile/agents")]
    public class AgentsController : Controller
    {
		private OrdersDbContext db;
		public AgentsController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		public IActionResult Get()
		{
			return Ok(db.agents.Where(s=>s.dep == "19").Select(s => new { s.name, s.id }).OrderBy(s=>s.name));
		}
	}
}