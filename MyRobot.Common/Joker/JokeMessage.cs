using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using MyRobot.Alexa;
using MyRobot.Common;
using System.ComponentModel.Composition;
using System.Text;

namespace MyRobot.Joker
{
    [Export(typeof(IMessage))]
    [ExportMetadata("Intent", "JokeIntent")]
    class JokeMessage : BaseMessage
    {
        public override SkillResponse Process(IntentRequest request)
        {
            var responseText = new StringBuilder();
            //We will use SSML as reponse format
            //https://developer.amazon.com/docs/custom-skills/speech-synthesis-markup-language-ssml-reference.html
            responseText.Append("<speak>");
            JokesRepository repo = new JokesRepository();
            var categorySlot = request.Intent.Slots["JokeCategory"];
            var category = string.Empty;
            string jokeCategory = categorySlot.Value;
            if(jokeCategory!=null)
            {
                //Performing similarity search!
                jokeCategory = JokesRepository.SelectProperCategory(jokeCategory);
            }
            var joke = JokesRepository.NextJoke(jokeCategory);

            if (joke != null)
            { 
                responseText.Append("<audio src = 'https://s3.amazonaws.com/ask-soundlibrary/ui/gameshow/amzn_ui_sfx_gameshow_positive_response_01.mp3'/>");
                responseText.Append(joke.JokeText);
                responseText.Append("<break/>");
            }
            else
            {
                responseText.Append("<audio src='soundbank://soundlibrary/cartoon/amzn_sfx_boing_long_1x_01'/>");
                responseText.Append(Ssml.SayAs("Ops! No Joke found!", "interjection"));
                responseText.Append("<break/>");
            }

            responseText.Append("</speak>");
            SsmlOutputSpeech speech = new SsmlOutputSpeech();
            speech.Ssml = responseText.ToString();
            return ResponseBuilder.Tell(speech);
        }
    }
}
