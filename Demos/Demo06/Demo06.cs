using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo06
{
    public static class Demo06
    {
        [FunctionName("Demo06")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            dynamic alexaRequestJson = await req.Content.ReadAsAsync<object>();
            string requestType = alexaRequestJson.request.type;

            string speechText = string.Empty;

            switch (requestType)
            {
                case "LaunchRequest":
                    speechText = "Willkommen bei unserem Skill";
                    break;

                case "IntentRequest":

                    string intentName = alexaRequestJson.request.intent.name;
                    switch (intentName)
                    {
                        case "Weather":
                            speechText = "Morgen wird das Wetter bestimmt besser als heute";
                            break;
                        case "Appointment":
                            speechText = "Kann schon sein, aber mir hat keiner Bescheid gegeben";
                            break;
                    }

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
