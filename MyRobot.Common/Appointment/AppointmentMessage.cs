using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using MyRobot.Alexa;
using MyRobot.Common;
using MyRobot.ServiceBus;
using System.ComponentModel.Composition;
using System.Text;

namespace MyRobot.Joker
{
    [Export(typeof(IMessage))]
    [ExportMetadata("Intent", "AppointmentIntent")]
    class AppointmentMessage : BaseMessage
    {
        public override SkillResponse Process(IntentRequest request)
        {
            var responseText = new StringBuilder();
            //We will use SSML as reponse format
            //https://developer.amazon.com/docs/custom-skills/speech-synthesis-markup-language-ssml-reference.html
            responseText.Append("<speak>");

            var helper = new QueueHelper<Appointment>(
                       System.Environment.GetEnvironmentVariable("AppointmentQueueConnection", System.EnvironmentVariableTarget.Process));
            var appointment = helper.Receive();
            
            if (appointment != null)
            { 
                responseText.Append("<audio src = 'https://s3.amazonaws.com/ask-soundlibrary/ui/gameshow/amzn_ui_sfx_gameshow_positive_response_01.mp3'/>");
                responseText.Append(Ssml.SayAs(appointment.Subject, "interjection"));
                responseText.Append(Ssml.SayAs(appointment.Start,true));
                responseText.Append("<break/>");
            }
            else
            {
                responseText.Append("<audio src='soundbank://soundlibrary/cartoon/amzn_sfx_boing_long_1x_01'/>");
                responseText.Append(Ssml.SayAs("You got all appointments! Get to work!", "interjection"));
                responseText.Append("<break/>");
            }

            responseText.Append("</speak>");
            SsmlOutputSpeech speech = new SsmlOutputSpeech();
            speech.Ssml = responseText.ToString();
            return ResponseBuilder.Tell(speech);
        }
    }
}
