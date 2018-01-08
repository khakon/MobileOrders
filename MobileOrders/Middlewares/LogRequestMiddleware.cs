using Microsoft.AspNetCore.Http;
using MobileOrders.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MobileOrders.Middlewares
{
    public class LogRequestMiddleware
    {
		private readonly RequestDelegate _next;

		public LogRequestMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, LogRequest logRequest)
		{
			var requestBodyStream = new MemoryStream();
			var originalRequestBody = context.Request.Body;
			await context.Request.Body.CopyToAsync(requestBodyStream);
			requestBodyStream.Seek(0, SeekOrigin.Begin);
			logRequest.WriteLogRequest(requestBodyStream, context.Request.Headers);
			requestBodyStream.Seek(0, SeekOrigin.Begin);
			context.Request.Body = requestBodyStream;

			await _next.Invoke(context);
			context.Request.Body = originalRequestBody;
		}
	}
}
