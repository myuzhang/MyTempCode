using System.Threading.Tasks;
using System.Web.Http;
using GarminIntegration;
using Newtonsoft.Json.Linq;
using WebApi.Models;

namespace WebApi.Controllers
{
    [MyRoutePrefix("garminnotification")]
    public class GarminNotificationController : ApiController
    {
        private readonly GarminConnector _garminConnector;

        public GarminNotificationController(GarminConnector garminConnector)
        {
            _garminConnector = garminConnector;
        }

        // POST: api/1.0/garminnotification/dailies
        [Route("dailies")]
        [HttpPost]
        public async Task<IHttpActionResult> DailiesNotification([FromBody] JObject jObject)
        {
            var notification = jObject.ToObject<DailiesPing>();
            foreach (var daily in notification.Dailies)
            {
                await Task.Delay(100);
            }
            return Ok();
        }

        // POST: api/1.0/garminnotification/activities
        [Route("activities")]
        [HttpPost]
        public async Task<IHttpActionResult> ActivitiesNotification([FromBody] JObject jObject)
        {
            var notification = jObject.ToObject<ActivitiesPing>();
            await Task.Delay(1000); // todo: change to collect data from garmin
            return Ok();
        }
    }
}
