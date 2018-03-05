using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo05
{
    public static class Demo05
    {
        // Implement the Built-in Intents
        // https://developer.amazon.com/de/docs/custom-skills/implement-the-built-in-intents.html

        [FunctionName("Demo05")]
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
                        case "AMAZON.HelpIntent":
                            speechText = "Eine Hilfe zur Verwendung des Skills";
                            break;
                        case "AMAZON.CancelIntent":
                            speechText = "Eine Reaktion auf Abbrechen";
                            break;
                        case "AMAZON.StopIntent":
                            speechText = "Eine Reaktion auf Stopp";
                            break;
                        case "Test":
                            speechText = "Der Test-Intent wurde aufgerufen";
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
