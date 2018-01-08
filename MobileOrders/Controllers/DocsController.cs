using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;


namespace MobileOrders.Controllers
{
	[Authorize]
	public class DocsController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public DocsController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[Route("api/Docs")]
		public IQueryable Get()
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			var model = db.balDocs.Where(s => s.agent == agent.id);
            return model;
        }
    }
}
