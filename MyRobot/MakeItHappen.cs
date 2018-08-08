using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace MyRobot
{
    public static class MakeItHappen
    {
        [FunctionName("MakeItHappen")]
        public async static Task<SkillResponse> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, 
            new string[] { "get", "post" }, Route = null)]
            SkillRequest req, TraceWriter log)
        {
            var intent = req.Request as IntentRequest;
            if (intent!=null)
            {
                
                return ResponseBuilder.TellWithCard(
                    intent.Intent.Name,"My Robot", intent.Intent.Name);
            }


            return GetDefault();


        //    return DefaultRequest(req);
        /*
        // Simple Function
        // Get request body
        dynamic data = await req.Content.ReadAsAsync<object>();
        log.Info($"Content={data}");
        if (data.request.type == "LaunchRequest")
        {
            // default launch request, let's just let them know what you can do
            log.Info($"Default LaunchRequest made");
            return DefaultRequest(req);
        }
        else if (data.request.type == "IntentRequest")
        {
            // Set name to query string or body data
            string intentName = data.request.intent.name;
            log.Info($"intentName={intentName}");
                switch (intentName)
                {
                    case "AddIntent":
                        var n1 = Convert.ToDouble(data.request.intent.slots["firstnum"].value);
                        var n2 = Convert.ToDouble(data.request.intent.slots["secondnum"].value);
                        double result = n1 + n2;
                        string subject = result.ToString();
                        return req.CreateResponse(HttpStatusCode.OK, new
                        {
                            version = "1.0",
                            sessionAttributes = new { },
                            response = new
                            {
                                outputSpeech = new
                                {
                                    type = "PlainText",
                                    text = $"The result is {result.ToString()}."
                                },
                                card = new
                                {
                                    type = "Simple",
                                    title = "Alexa-Azure Simple Calculator",
                                    content = $"The result is {result.ToString()}."
                                },
                                shouldEndSession = true
                            }
                        });
                    // Add more intents and default responses
                    default:
                        return DefaultRequest(req);
                }
        return DefaultRequest(req);
        }
        else
        {
            return DefaultRequest(req);
        }*/
        }

        private static SkillResponse GetDefault()
        {
            // create the speech response - cards still need a voice response
            var speech = new Alexa.NET.Response.SsmlOutputSpeech();
            speech.Ssml = "<speak>Welcome to MyRobot! Your wish is an order.</speak>";

            // create the card response
            var finalResponse = ResponseBuilder.TellWithCard(speech, "My Robot", "Welcome to MyRobot! Your wish is an order.");
            return finalResponse;
        }

    }
}
