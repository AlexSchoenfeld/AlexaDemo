using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo07
{
    public static class Demo07
    {
        // Best Practices for Sample Utterances and Custom Slot Type Values
        // https://developer.amazon.com/de/docs/custom-skills/best-practices-for-sample-utterances-and-custom-slot-type-values.html


        [FunctionName("Demo07")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            dynamic alexaRequestJson = await req.Content.ReadAsAsync<object>();
            string requestType = alexaRequestJson.request.type;

            string speechText = string.Empty;
            bool endSession = false;

            switch (requestType)
            {
                case "LaunchRequest":
                    speechText = "Willkommen bei unserem Skill";
                    break;

                case "IntentRequest":

                    string intentName = alexaRequestJson.request.intent.name;
                    switch (intentName)
                    {
                        case "Horoscope":
                            string slotValue = alexaRequestJson.request.intent.slots.Sign.value;
                            speechText = $"Hier das Horoskop für {slotValue}";
                            endSession = true;
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
                    shouldEndSession = endSession
                }
            });
        }
    }
}
