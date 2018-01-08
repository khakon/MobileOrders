using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MobileOrders.Helpers
{
    public class LogRequest
    {
		public void WriteLogRequest(MemoryStream body, IHeaderDictionary headers)
		{
			using (System.IO.StreamWriter fileLog =
			new System.IO.StreamWriter(@"D:\order\log\request\request" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt"))
			{
				foreach(var item in headers)
				{
					fileLog.WriteLine($"{item.Key}: {item.Value}" );
				}
			}
			using (var fileStream = File.Create(@"D:\order\log\request\requestBody" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt"))
			{
				body.CopyTo(fileStream);
			}
		}
	}
}
