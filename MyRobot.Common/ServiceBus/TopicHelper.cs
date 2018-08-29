using Microsoft.ServiceBus.Messaging;


namespace MyRobot.ServiceBus
{
    public class TopicHelper<MessageType>
    {
        /*  Usage Sample
            var topicHelper = new TopicHelper<StatusMessage>(
                                    System.Environment.GetEnvironmentVariable("ServiceBusConnectionString", System.EnvironmentVariableTarget.Process),
                                    System.Environment.GetEnvironmentVariable("TeamCityTopicName", System.EnvironmentVariableTarget.Process));
            topicHelper.Send(this);*/

        private string _connectionString;
        private string _topicName;
        private static TopicClient _topicClient;

        public TopicHelper(string connectionString, string topicName )
        {
            _topicName = topicName;
            _connectionString = connectionString;
            //To send messages, we need a Topic Client!!
            _topicClient = TopicClient.CreateFromConnectionString(_connectionString, _topicName);
        }

        public void Send(MessageType message)
        {
            var topicMessage = new BrokeredMessage(message);
            _topicClient.Send(topicMessage);
        }

    }
}
