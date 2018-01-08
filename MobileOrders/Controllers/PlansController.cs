using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace MobileOrders.Controllers
{
	[Authorize]
	public class PlansController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public PlansController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[Route("api/Plans")]
		public IQueryable Get()
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			try
            {
                return db.Plans.Where(s => s.agent == agent.id);
            }
            catch(Exception ex)
            {
                return new List<Plan>().AsQueryable();
            }
        }
    }
}
