using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Api.Models;

namespace Api.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        // GET: api/Product
        public IEnumerable<image> Get()
        {
            using (StevenCorreia_dbEntities context = new StevenCorreia_dbEntities())
            {
                return context.images.ToList();
            }
        }

        // GET: api/Product/5
        public image Get(string id)
        {
            using (StevenCorreia_dbEntities context = new StevenCorreia_dbEntities())
            {
                return context.images.Find(id);
            }
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
