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
	public class ItemsController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public ItemsController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[Route("api/Items")]
		public IQueryable Get()
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			var dep = db.agents.Where(s => s.id == agent.id).FirstOrDefault().dep;
            var groups = db.GoodsAgents.Where(s => s.dep == dep);
            var list = db.Groups.Where(s => groups.Any(g => s.tree.Contains(g.code)) || groups.Any(t => s.GroupId == t.code));
            var root = db.Groups.Where(s => (list.Any(g => g.tree.Contains(s.GroupId)) || list.Any(g => g.GroupId == s.GroupId)));
            return db.Items.Where(s => root.Any(r => s.tree.Contains(r.GroupId)));
        }

    }
}
