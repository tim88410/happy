using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace SlnBlogAndShop.Controllers
{
    public class webApiController : ApiController
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        // GET: api/webApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/webApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/webApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/webApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/webApi/5
        public void Delete(int id)
        {
        }
    }
}
