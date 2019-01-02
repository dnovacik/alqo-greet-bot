using System;
using System.Collections.Generic;
using System.Text;

namespace AlqoGreetBot.Common.Extensions
{
    public static class Extensions
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string ParseUnixTimeToDateTimeString(this int unixTime)
        {
            if (unixTime != 0)
            {
                return _epoch.AddSeconds(unixTime).ToString();
            }
            else
            {
                return "-";
            }
        }
    }
}
