using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;

namespace MobileOrders.Controllers.Mobile
{
	[Produces("application/json")]
	public class OrdersController : Controller
	{
		private OrdersDbContext db;
		public OrdersController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[HttpGet]
		[Route("mobile/orders/{id}")]
		public IActionResult Get(string id)
		{
			return Ok(db.historyOrders.Where(s=>s.agent == id).GroupBy(s=> new {s.period, s.iddoc, s.docno, s.kontr }).Select(s => new { docDate = s.Key.period, customer = s.Key.kontr, idDoc = s.Key.docno, id = s.Key.iddoc, sum = s.Sum(t=>t.suma) }).OrderByDescending(s => s.docDate).ThenBy(s=>s.customer));
		}
		[HttpGet]
		[Route("mobile/orderItems/{id}")]
		public IActionResult GetItems(string id)
		{
			return Ok(db.historyOrders.Where(s => s.docno == id).Select(s => new { docDate = s.period, customer = s.kontr, idDoc = s.id, sum = s.suma, product = s.tov, price = s.price, quant = s.quant }).OrderBy(s => s.product));
		}
	}
}