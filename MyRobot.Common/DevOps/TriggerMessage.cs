using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace MyRobot.Common.DevOps
{
    [Export(typeof(IMessage))]
    [ExportMetadata("Intent", "TriggerIntent")]
    public class TriggerMessage:BaseMessage
    {
        public string BuildName { get; set; }

        public override SkillResponse Process(IntentRequest request)
        {
            return ResponseBuilder.Tell("Trigger");
        }
    }
}
