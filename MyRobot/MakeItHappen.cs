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
using MyRobot.Common;

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
                MessageHandler handler = new MessageHandler();
                return handler.Handle(intent);               
            }

            return GetDefault();
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
