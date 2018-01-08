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
	[Authorize]
	[Produces("application/json")]
    public class GroupsController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public GroupsController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[Route("api/Groups")]
		public IQueryable Get()
		{
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			var dep = agent.dep;
			var groups = db.GoodsAgents.Where(s => s.dep == dep);
			var list = db.Groups.Where(s => groups.Any(g => s.tree.Contains(g.code)) || groups.Any(t => s.GroupId == t.code));
			var root = db.Groups.Where(s => (list.Any(g => g.tree.Contains(s.GroupId)) || list.Any(g => g.GroupId == s.GroupId)));
			return root;
		}

		[Route("api/Groups/{id}")]
		public IQueryable Get(string id)
		{
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			var dep = agent.dep;
			var groups = db.GoodsAgents.Where(s => s.dep == dep);
			var list = db.Groups.Where(s => groups.Any(g => s.tree.Contains(g.code)));
			var root = db.Groups.Where(s => s.ParentId == id && (list.Any(g => g.tree.Contains(s.GroupId)) || list.Any(g => g.GroupId == s.GroupId)));
			return root;
		}
	}
}