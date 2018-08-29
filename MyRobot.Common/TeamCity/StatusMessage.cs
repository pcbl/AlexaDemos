using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using MyRobot.Common;
using MyRobot.Alexa;

namespace MyRobot.TeamCity
{
    [Export(typeof(IMessage))]
    [ExportMetadata("Intent", "StatusIntent")]
    public class StatusMessage:BaseMessage
    {
        public override SkillResponse Process(IntentRequest request)
        {
            var responseText = new StringBuilder();
            //We will use SSML as reponse format
            //https://developer.amazon.com/docs/custom-skills/speech-synthesis-markup-language-ssml-reference.html
            responseText.Append("<speak>");

            //let´s connect as guest on Teamcity Public instance!
            var client = new TeamCitySharp.TeamCityClient("teamcity.jetbrains.com",true);
            client.ConnectAsGuest();

            //By default, when a list of entities is requested, only basic fields are included into the response. 
            //When a single entry is requested, all the fields are returned. 
            //The complex field values can be returned in full or basic form, depending on a specific entity.
            //https://confluence.jetbrains.com/display/TCD18/REST+API#RESTAPI-FullandPartialResponses
            string projectName = request.Intent.Slots["ProjectName"].Value;
            var project = client.Projects.ByName(projectName);
            if (project != null)
            {
                responseText.Append("<audio src = 'https://s3.amazonaws.com/ask-soundlibrary/ui/gameshow/amzn_ui_sfx_gameshow_positive_response_01.mp3'/>");
                responseText.AppendFormat("Here is the Status of the {0} Project:", Ssml.SayAs(project.Name, "interjection"));
                responseText.Append("<break/>");
                #region Build Information
                //Last Build informarion (we get only 1)
                //state: <queued/running/finished>
                //https://confluence.jetbrains.com/display/TCD18/REST+API
                var lastBuilds = client.Builds.AffectedProject(project.Id, 1, new List<string>() { "state:finished" });
                if (lastBuilds.Any())
                {
                    //We get by ID to load the full information!
                    var lastBuild = client.Builds.ById(lastBuilds.First().Id);

                    var triggeredBy = string.Empty;
                    if(lastBuild.Triggered.Type.Equals("schedule"))
                        triggeredBy = "automatically";
                    else if (lastBuild.Triggered.Type.Equals("schedule"))
                        triggeredBy = "by the " + Ssml.SayAs("Version Control System", "interjection") ;
                    else
                        triggeredBy = "by " + Ssml.SayAs(lastBuild.Triggered.User.Name, "interjection");
                    
                    //let us find how long ago was the build
                    responseText.AppendFormat("Last Build, {0}, triggered {1}, happened {2}, {3}, with {4} status",
                        Ssml.SayAs(lastBuild.BuildType.Name, "interjection"),
                        triggeredBy,
                        Ssml.SayAs(lastBuild.FinishDate,true),
                        lastBuild.FinishDate.TimeAgo(),
                        Ssml.SayAs(lastBuild.Status, "interjection"));

                    responseText.Append("<break/>");
                    
                    //Let´s collect statistics!
                    var buildStatistics = client.Statistics.GetByBuildId(lastBuild.Id);
                    //Default Statistics Values Provided by TeamCity
                    //https://confluence.jetbrains.com/display/TCD18/Custom+Chart#CustomChart-listOfDefaultStatisticValues
                    var failedTestCount = buildStatistics.FirstOrDefault(item => item.Name.Equals("FailedTestCount"));
                    var totalTestCount = buildStatistics.FirstOrDefault(item=>item.Name.Equals("TotalTestCount"));
                    if (totalTestCount!=null)
                    {
                        if (failedTestCount != null)
                        {
                            int failed = Convert.ToInt32(failedTestCount);
                            int total = Convert.ToInt32(totalTestCount);
                            if (failed > 0)
                            {
                                responseText.AppendFormat("All {0} tests passed!", Ssml.SayAs(total));
                            }
                            else
                            {
                                responseText.AppendFormat("{0} of {1} tests did not passed", Ssml.SayAs(Ssml.SayAs(failed), "interjection"), Ssml.SayAs(total));
                            }
                        }
                    }      
                    else
                    {
                        responseText.Append("No automated Tests were executed.");
                    }
                }
                else
                {
                    responseText.AppendFormat("No Build Information found for {0}", projectName);
                }
                #endregion
            }
            
            else
            {
                responseText.AppendFormat("{0} Project not found", projectName);
            }
            responseText.Append("That´s all. Bye.");
            responseText.Append("<audio src='https://s3.amazonaws.com/ask-soundlibrary/ui/gameshow/amzn_ui_sfx_gameshow_neutral_response_03.mp3'/>");
            responseText.Append("</speak>");
            SsmlOutputSpeech speech = new SsmlOutputSpeech();
            speech.Ssml = responseText.ToString();
            return ResponseBuilder.Tell(speech);

        }
    }
}
