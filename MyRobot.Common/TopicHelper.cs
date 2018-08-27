using Microsoft.ServiceBus.Messaging;


namespace MyRobot
{
    public class TopicHelper<MessageType>
    {
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
