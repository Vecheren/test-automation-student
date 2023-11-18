using System;
using System.Linq;

namespace VacationTests
{
    public static class DateTimeTupleExtensions
    {
        public static string ToString(this (DateTime, DateTime) startAndEndDate, string divider)
        {
            return string.Join(divider, new[] { startAndEndDate.Item1, startAndEndDate.Item2 }
                .Select(x => x.ToString("dd.MM.yyyy")));
        }
    }
}