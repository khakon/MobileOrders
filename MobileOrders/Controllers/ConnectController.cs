using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrdersRepository;
using System;
using System.Linq;


namespace MobileOrders.Controllers
{
	[Authorize]
	public class ConnectController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public ConnectController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
		}

		[HttpPost]
        public string Save(string model)
        {
            try
            {
                using (System.IO.StreamWriter fileLog =
                new System.IO.StreamWriter(@"D:\order\log\" + agent.name + ".txt"))
                {
                    fileLog.WriteLine("{0}", model);
                }
                return "Записано";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

		public string SaveFromFile(string file)
        {
            string model;
            using (System.IO.StreamReader fileStr =
            new System.IO.StreamReader(@"D:\order\log\" + file + ".txt"))
            {
                model = fileStr.ReadToEnd();
            }
            try
            {
                var log = new logsString { id = 0, agent = "manual", content = model, date = DateTime.Now };
                db.Entry(log).State = EntityState.Added;
                db.SaveChanges();
                var logs = JsonConvert.DeserializeObject<log[]>(model);
                foreach (var item in logs.ToList())
                {
                    item.timeStampServer = DateTime.Now;
                    db.Entry(item).State = EntityState.Added;
                    db.SaveChanges();
                }
                var orders = JsonConvert.DeserializeObject<Order[]>(model);
                foreach (var a in orders.ToList())
                {
                    foreach (var c in db.Orders.Where(o => o.agent == a.agent && o.customer == a.customer && o.number == a.number).ToList())
                    {
                        db.Entry(c).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                }
                foreach (var item in orders)
                {
                    db.Entry(item).State = EntityState.Added;
                    db.SaveChanges();
                }
                return "Записано";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
