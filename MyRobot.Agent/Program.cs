using MyRobot.Common;
using MyRobot.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var nextAppointment = LookForNextAppointment("OfficeDay");

            if (nextAppointment != null)
            {
                var helper = new QueueHelper<Appointment>(
                                     System.Environment.GetEnvironmentVariable("AgentConnectionString", System.EnvironmentVariableTarget.Process));
                helper.Send(nextAppointment);

                Console.WriteLine($"{nextAppointment.Subject} -> {nextAppointment.Start} :: MESSAGE SENT!!!");
            }
            else
            {
                Console.WriteLine("No Appointment found!");
            }
            Console.ReadKey();

            //AgentConnectionString - Get primary ConnectioNString from Azure Queue....

        }


        private static Appointment LookForNextAppointment(string textToSearch)
        {
            Microsoft.Office.Interop.Outlook.Application oApp = null;
            Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder CalendarFolder = null;
            Microsoft.Office.Interop.Outlook.Items outlookCalendarItems = null;

            oApp = new Microsoft.Office.Interop.Outlook.Application();
            mapiNamespace = oApp.GetNamespace("MAPI"); ;
            CalendarFolder = mapiNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
            outlookCalendarItems = CalendarFolder.Items;
            outlookCalendarItems.Sort("[Start]");
            var now = DateTime.Now.Date.ToString("g");
            var nowIn1Year = DateTime.Now.Date.AddYears(1).ToString("g");
            outlookCalendarItems = outlookCalendarItems.Restrict($"[Start] >= \"{now}\" AND [End] < \"{nowIn1Year}\" ");

            foreach (Microsoft.Office.Interop.Outlook.AppointmentItem item in outlookCalendarItems)
            {
                var subject = item.Subject;
                if (subject.IndexOf(textToSearch,StringComparison.InvariantCultureIgnoreCase)!=-1)
                {
                    return new Appointment() { Subject = subject, Start = item.Start };
                }
            }
            return null;
        }
    }
}
