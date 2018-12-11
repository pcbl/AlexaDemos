using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.ServiceBus
{
    public class QueueHelper<MessageType>
    {
        /*  Usage Sample
         To listen: AppointmentQueueConnection << Replace with your Azure one
         var helper = new QueueHelper<StatusMessage>(
                                 System.Environment.GetEnvironmentVariable("AgentConnectionString", System.EnvironmentVariableTarget.Process));
         helper.Send(this);*/

        private string _connectionString;
        private static QueueClient _queueClient;

        public QueueHelper(string connectionString)
        {
            _connectionString = connectionString;
            //To send messages, we need a Queue Client!!
            _queueClient = QueueClient.CreateFromConnectionString(connectionString);
        }

        public void Send(MessageType message)
        {
            var queueMessage = new BrokeredMessage(message);
            _queueClient.Send(queueMessage);
        }

        public MessageType Receive()
        {      
           var message = _queueClient.Receive(TimeSpan.FromMilliseconds(500));//We wait half milesecond!
            if (message != null)
            {
                var body = message.GetBody<MessageType>();
                message.CompleteAsync();//Completing to remove from queue
                return body;
            }
            else
            {
                return default(MessageType);
            }
        }
    }
}
