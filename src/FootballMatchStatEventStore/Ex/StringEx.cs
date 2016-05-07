using System;

namespace FootballMatchStatEventStore.Ex
{
    public static class StringEx
    {
        public static Guid ToGuid(this string stringGuid)
        {
            return Guid.Parse(stringGuid);
        }
    }
}
