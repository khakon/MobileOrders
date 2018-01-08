using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;


namespace MobileOrders.Controllers
{
	[Authorize]
	public class HistoryController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public HistoryController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[Route("api/Invoices")]
		public IQueryable Get()
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			return db.history.Where(s => s.agent == agent.id);
        }
    }
}
