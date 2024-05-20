using System;

namespace CurrentTime
{
    public class Time
    {
        public long UnixTime()
        {
            // Get current time in UTC
            DateTimeOffset nowUtc = DateTimeOffset.UtcNow;

            // Convert to Unix timestamp (milliseconds)
            long unixTimeInMilliseconds = nowUtc.ToUnixTimeMilliseconds();
            return unixTimeInMilliseconds;
        }
        public string TimeString()
        {
            return DateTime.Now.ToString("dddd, MMMM dd, yyyy h:mm:ss tt");
        }
    }
}