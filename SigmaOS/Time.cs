using System;

namespace CurrentTime
{
    public class Time
    {
        public long UnixTimeMilliseconds()
        {
            // Get current time in UTC
            DateTimeOffset nowUtc = DateTimeOffset.UtcNow;

            // Convert to Unix timestamp (milliseconds)
            long unixTimeInMilliseconds = nowUtc.ToUnixTimeMilliseconds();
            return unixTimeInMilliseconds;
        }
        public long UnixTimeSeconds()
        {
            DateTimeOffset nowUtc = DateTimeOffset.UtcNow;
            return nowUtc.ToUnixTimeSeconds();
        }
        public string TimeString()
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int hour = now.Hour;
            int minute = now.Minute;
            int second = now.Second;

            string[] daysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            string dayOfWeek = daysOfWeek[(int)now.DayOfWeek];
            string monthName = months[month - 1];
            string amPm = hour >= 12 ? "PM" : "AM";
            hour = hour % 12;
            hour = hour == 0 ? 12 : hour;

            return $"{dayOfWeek}, {monthName} {day}, {year} {hour}:{minute:D2}:{second:D2} {amPm}";
        }
    }
}