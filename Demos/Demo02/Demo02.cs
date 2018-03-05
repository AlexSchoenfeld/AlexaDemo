using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo2
{
    public static class Demo02
    {
        [FunctionName("Demo2")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.1",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Hallo Welt!"
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Unsere Demo",
                        content = "Hallo Welt!"
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}
