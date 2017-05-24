using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using WebApi.Models;

namespace WebApi.Controllers
{
    [MyRoutePrefix("patient")]
    public class PatientController : ApiController
    {
        // GET: api/Patient
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [Route("{id}")]
        [HttpGet]
        public Test Get(int id)
        {
            return new Test {Age = id, Name = "Hello My"};
        }

        // POST: api/Patient
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] JObject jObject)
        {
            var j = jObject;
            await Task.Delay(1000);
            return Ok();
        }

        // PUT: api/Patient/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Patient/5
        public void Delete(int id)
        {
        }
    }
}
