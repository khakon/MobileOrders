using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileOrders.Models
{
    public class LoginModel
    {
		public string agentCode { get; set; }
		public string password { get; set; }
		public string mobileID { get; set; }
	}
}
