using System;
using System.Collections.Generic;

namespace ControlHorario.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static IEnumerable<DateTime> EachDay(this DateTime from, DateTime to)
        {
            var current = new DateTime(from.Year, from.Month, from.Day);
            while (current <= to)
            {
                yield return current;
                current = current.AddDays(1);
            }
        }
    }
}
