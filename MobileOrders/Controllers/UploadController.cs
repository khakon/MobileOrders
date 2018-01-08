using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebOrders.Utils;

namespace MobileOrders.Controllers
{
    public class UploadController : ApiController
    {
        public string Get()
        {
            try
            {
                SQLProcedure.StoredProcedure("Upload");
                return "Загружено";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
