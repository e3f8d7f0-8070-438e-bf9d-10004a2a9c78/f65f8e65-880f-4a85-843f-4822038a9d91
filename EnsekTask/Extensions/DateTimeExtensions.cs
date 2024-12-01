namespace EnsekTask.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1).ToUniversalTime();

        public static int ToTimestamp(this DateTime when)
        {
            return (int)(when - _epoch).TotalSeconds;
        }

        public static DateTime FromTimestamp(this int timestamp)
        {
            var when = _epoch.AddSeconds(timestamp);
            return when;
        }
    }
}
