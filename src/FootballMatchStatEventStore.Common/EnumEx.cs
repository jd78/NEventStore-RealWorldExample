using System;

namespace FootballMatchStatEventStore.Common
{
    public static class EnumEx
    {
        /// <summary>
        /// Maps anything to a enum value if a correspondence exists, throws exception otherwise.
        /// </summary>
        /// <returns></returns>
        public static TOut MapByStringValue<TIn, TOut>(TIn x, bool ignoreCase = false)
            where TOut : struct
        {
            var stringValue = x.ToString();
            var y = (TOut)Enum.Parse(typeof(TOut), stringValue, ignoreCase);

            return y;
        }
    }
}
