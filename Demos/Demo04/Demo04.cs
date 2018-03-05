using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo04
{
    public static class Demo04
    {
        // Request Types Reference (LaunchRequest, IntentRequest, SessionEndedRequest)
        // https://developer.amazon.com/de/docs/custom-skills/request-types-reference.html


        [FunctionName("Demo04")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            string speechText = string.Empty;
            dynamic alexaRequestJson = await req.Content.ReadAsAsync<object>();

            string requestType = alexaRequestJson.request.type;
            switch (requestType)
            {
                case "LaunchRequest":
                    speechText = "Willkommen bei unserem Skill";
                    break;

                case "IntentRequest":
                    speechText = "Dazu fällt mir leider nichts ein...";
                    break;

                case "SessionEndedRequest":
                    // hier landen wir beim Ende der Session, können aber keine Antwort mehr senden
                    log.Info("SessionEndedRequest received");
                    break;
            }

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.1",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = speechText
                    },
                    shouldEndSession = false
                }
            });
        }
    }
}
