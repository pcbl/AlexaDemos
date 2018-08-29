using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.Alexa
{
    public static class Ssml
    {
        /// <summary>
        /// https://stackoverflow.com/questions/11/calculate-relative-time-in-c-sharp
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>Returns the ellapsed time on a nice format(SSML)</returns>
        public static string TimeAgo(this DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? SayAs("one", "number") + " second ago" : SayAs(ts.Seconds) + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return SayAs(ts.Minutes) + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return SayAs(ts.Hours) + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return SayAs(ts.Days) + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? SayAs("one", "number") + " month ago" : SayAs(months) + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? SayAs("one", "number") + " year ago" : SayAs(years) + " years ago";
            }
        }

        public static string SayAs(string value, string format)
        {
            return $"<say-as interpret-as=\"{format}\">{value}</say-as>";
        }

        public static  string SayAs(int value)
        {
            return SayAs(value.ToString(), "number");
        }

        public static string SayAs(DateTime value, bool sayTime = false)
        {
            return SayAs(value.ToString("yyyyMMdd"), "date") + (sayTime ? " at " + SayAs(value.ToString("HH:mm"), "time") : string.Empty);
        }
    }
}
