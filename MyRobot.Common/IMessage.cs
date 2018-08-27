using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace MyRobot.Common
{
    public interface IMessage
    {
        SkillResponse Process(IntentRequest request);
    }
}
