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
	public class PricesController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public PricesController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[Route("api/Prices")]
		public IQueryable Get()
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			var dep = db.agents.Where(s => s.id == agent.id).FirstOrDefault().dep;
            var groups = db.GoodsAgents.Where(s => s.dep == dep);
            var list = db.Groups.Where(s => groups.Any(g => s.tree.Contains(g.code)) || groups.Any(t => s.GroupId == t.code));
            var root = db.Groups.Where(s => list.Any(g => g.tree.Contains(s.GroupId)) || list.Any(g => g.GroupId == s.GroupId));
            var items = db.Items.Where(s => root.Any(r => s.tree.Contains(r.GroupId)));
            var customers = db.Customers.Where(m => m.idAgent.Trim() == agent.id);
            var model = db.Prices.Where(s => (items.Any(i => i.ItemId == s.TovId) || root.Any(i => i.GroupId == s.TovId)) && customers.Any(c => c.idKontr == s.KnId)).Select(s => new { KnId = s.KnId, TovId = s.TovId, Price = s.Price1, Discount = s.Discount });
            return model;
        }
    }
}
