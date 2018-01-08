using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrdersRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MobileOrders.Controllers
{
	[Authorize]
	public class PaymentsController : Controller
    {
		private OrdersDbContext db;
		private agent agent;
		public PaymentsController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		// GET: Payments
		[HttpPost]
        public string Save(string model)
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			using (System.IO.StreamWriter fileLog =
            new System.IO.StreamWriter(@"D:\order\logPayment\" + agent.name + ".txt"))
            {
                fileLog.WriteLine("{0}", model);
            }
            try
            {
                var log = new logsString { id = 0, agent = agent.name, content = model, date = DateTime.Now };
                db.Entry(log).State = EntityState.Added;
                db.SaveChanges();
                var logs = JsonConvert.DeserializeObject<logsPayment[]>(model);
                foreach (var item in logs.ToList())
                {
                    item.timeStampServer = DateTime.Now;
                    db.Entry(item).State = EntityState.Added;
                    db.SaveChanges();
                }
                var pays = JsonConvert.DeserializeObject<Payment[]>(model);
                foreach (var a in pays.ToList())
                {
                    foreach (var c in db.Payments.Where(o => o.agent == a.agent && o.customer == a.customer).ToList())
                    {
                        db.Entry(c).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                }
                foreach (var item in pays)
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
        [HttpPost]
        public string Rescue(string model)
        {
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			try
            {
                using (System.IO.StreamWriter fileLog = new System.IO.StreamWriter(@"D:\order\rescue\payments\" + agent.name + ".txt"))
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
			agent = db.agents.FirstOrDefault(s => s.name == db.MobileUsers.FirstOrDefault(a => a.AgentMobileId == User.Identity.Name).AgentName);
			string model;
            using (System.IO.StreamReader fileStr =
            new System.IO.StreamReader(@"D:\order\logPayment\" + file + ".txt"))
            {
                model = fileStr.ReadToEnd();
            }
            try
            {
                using (System.IO.StreamWriter fileLog =
                new System.IO.StreamWriter(@"D:\order\logPayment\" + agent.name + ".txt"))
                {
                    fileLog.WriteLine("{0}", model);
                }
                try
                {
                    var log = new logsString { id = 0, agent = agent.name, content = model, date = DateTime.Now };
                    db.Entry(log).State = EntityState.Added;
                    db.SaveChanges();
                    var logs = JsonConvert.DeserializeObject<logsPayment[]>(model);
                    foreach (var item in logs.ToList())
                    {
                        item.timeStampServer = DateTime.Now;
                        db.Entry(item).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    var pays = JsonConvert.DeserializeObject<Payment[]>(model);
                    foreach (var a in pays.ToList())
                    {
                        foreach (var c in db.Payments.Where(o => o.agent == a.agent && o.customer == a.customer).ToList())
                        {
                            db.Entry(c).State = EntityState.Deleted;
                            db.SaveChanges();
                        }
                    }
                    foreach (var item in pays)
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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}