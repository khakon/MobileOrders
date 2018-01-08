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
    public class InvoicesController : Controller
    {
		private OrdersDbContext db;
		public InvoicesController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[HttpGet]
		[Route("mobile/invoices/{id}")]
		public IActionResult Get(string id)
		{
			try
			{
				var model = db.historyNestle.Where(s => s.agent == id).GroupBy(s => new { s.period, s.iddoc, s.docno, s.orderId, s.kontr }).Select(s => new { docDate = s.Key.period, customer = s.Key.kontr, idDoc = s.Key.docno, order = s.Key.orderId, id = s.Key.iddoc, sum = s.Sum(t => t.suma) }).OrderByDescending(s => s.docDate).ThenBy(s => s.customer).ToList();
				return Ok(model);
			}
			catch(Exception ex)
			{
				return Ok(ex.Message);
			}
			
		}
		[HttpGet]
		[Route("mobile/invoiceItems/{id}")]
		public IActionResult GetItems(string id)
		{
			return Ok(db.history.Where(s => s.docno == id).Select(s => new { docDate = s.period, customer = s.kontr, idDoc = s.id, sum = s.suma, product = s.tov, price = s.suma/s.quant, quant = s.quant }).OrderBy(s => s.product));
		}
	}
}