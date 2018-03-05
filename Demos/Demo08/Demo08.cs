using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo08
{
    public static class Demo08
    {
        [FunctionName("Demo08")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            dynamic alexaRequestJson = await req.Content.ReadAsAsync<object>();
            string requestType = alexaRequestJson.request.type;

            string speechText = string.Empty;
            bool endSession = false;
            dynamic responseSessionAttributes = new { };

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
                            responseSessionAttributes = new
                            {
                                sign = slotValue
                            };
                            break;

                        case "HoroscopeLast":
                            string lastSign = alexaRequestJson.session?.attributes?.sign;
                            if (!string.IsNullOrEmpty(lastSign))
                            {
                                speechText = $"Zuletzt wurde nach {lastSign} gefragt";
                            }
                            else
                            {
                                speechText = "Daran kann ich mich leider nicht erinnern";
                            }
                            break;
                    }

                    break;
            }

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.1",
                sessionAttributes = responseSessionAttributes,
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
