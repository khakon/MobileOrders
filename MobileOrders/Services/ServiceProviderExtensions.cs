using Microsoft.Extensions.DependencyInjection;
using MobileOrders.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileOrders.Services
{
    public static class ServiceProviderExtensions
    {
		public static void AddLogRequest(this IServiceCollection services)
		{
			services.AddTransient<LogRequest>();
		}
	}
}
