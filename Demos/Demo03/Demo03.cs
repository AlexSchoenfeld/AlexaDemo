using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Demos.Demo3
{
    public static class Demo03
    {
        // Speech Synthesis Markup Language (SSML) Reference
        // https://developer.amazon.com/de/docs/custom-skills/speech-synthesis-markup-language-ssml-reference.html

        // Speechcon Reference(Interjections): German
        // https://developer.amazon.com/de/docs/custom-skills/speechcon-reference-interjections-german.html


        [FunctionName("Demo03")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            string speechText = "<speak>Hallo Welt!</speak>";
            // <speak>Soll ich euch ein Geheimnis verraten? <amazon:effect name="whispered">Ich bin gar kein Mensch</amazon:effect>! Unglaublich, oder?</speak>
            // <speak>1234567890</speak>
            // <speak><say-as interpret-as="digits">1234567890</say-as></speak>
            // <speak><say-as interpret-as="spell-out">Hallo</say-as></speak>
            // <speak>iiieh</speak>
            // <speak><say-as interpret-as="interjection">iiieh</say-as></speak>


            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.1",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "SSML",
                        ssml = speechText
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}
