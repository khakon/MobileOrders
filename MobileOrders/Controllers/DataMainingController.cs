using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MobileOrders.Controllers
{
    public class DataMainingController : Controller
    {
        DashboardsEntities db = new DashboardsEntities();        
        // GET: api/DataMaining
        public Dictionary<string, Dictionary<string, int>> GetCustomerAgent()
        {
            var maxDate = db.debts.Max(s => s.period);
            Dictionary<string, Dictionary<string, int>> list = new Dictionary<string, Dictionary<string, int>>();
            //var list = db.debts.Where(s => s.period == maxDate).Select(s => new { customer = s.customers.customer.Replace("і", "и"), balance = s.bal, debt = s.debt, overdue = s.customers.overdue });
            foreach (var a in db.debts.Where(s => s.period == maxDate).Select(s => s.agent).Distinct().ToList())
            {
                Dictionary<string, int> dict = db.debts.Where(s => s.period == maxDate && s.agent == a).Select(s => new { customer = s.customers.customer.Replace("і", "и"), overdue = (int)s.customers.overdue }).ToDictionary(m => m.customer.ToString(), m => m.overdue);
                list.Add(db.listagents.Where(t => t.id == a).FirstOrDefault().agent, dict);
            }
            return list;
        }

        // GET: api/DataMaining/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DataMaining
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DataMaining/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DataMaining/5
        public void Delete(int id)
        {
        }
    }
}
