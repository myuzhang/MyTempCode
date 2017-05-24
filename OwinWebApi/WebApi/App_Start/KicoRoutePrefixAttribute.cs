using System.Web.Http;

namespace WebApi
{
    public class MyRoutePrefixAttribute : RoutePrefixAttribute
    {
        public MyRoutePrefixAttribute(string prefix) : base($"api/{Constants.ApiVersion}/{prefix}")
        {
        }
    }
}