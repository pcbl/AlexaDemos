using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace MyRobot.Common
{
    public abstract class BaseMessage : IMessage
    {
        public abstract SkillResponse Process(IntentRequest request);
    }
}
