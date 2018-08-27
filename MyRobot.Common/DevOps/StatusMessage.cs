using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using TeamCitySharp.Locators;
namespace MyRobot.Common.DevOps
{
    [Export(typeof(IMessage))]
    [ExportMetadata("Intent", "StatusIntent")]
    public class StatusMessage:BaseMessage
    {
        public override SkillResponse Process(IntentRequest request)
        {
            /*  
               var topicHelper = new TopicHelper<StatusMessage>(
                  System.Environment.GetEnvironmentVariable("ServiceBusConnectionString", System.EnvironmentVariableTarget.Process),
                  System.Environment.GetEnvironmentVariable("TeamCityTopicName", System.EnvironmentVariableTarget.Process));

              topicHelper.Send(this);*/
            var client = new TeamCitySharp.TeamCityClient("teamcity.jetbrains.com",true);
            client.ConnectAsGuest();
            string projectName = "AceJump";
            foreach (var buildType in client.Projects.ByName(projectName).BuildTypes.BuildType)
            {                
                if(request.Intent.Slots["BuildName"].Value.Equals(buildType.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    var lastBuild = client.Builds.LastBuildByBuildConfigId(buildType.Id);
                    return ResponseBuilder.Tell($"{buildType.Name}@{projectName} - {lastBuild.Status}");
                }
            }

            return ResponseBuilder.Tell("Status");
        }
    }
}
