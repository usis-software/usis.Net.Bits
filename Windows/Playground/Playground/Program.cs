using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using usis.Platform;

namespace Playground
{
    internal static class Program
    {
        internal static void Main()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Playground.HolidaysData.plist"))
            {
                foreach (var holiday in LoadHolidaysFromPList(stream))
                {
                    Console.WriteLine(holiday.Name);
                    //if (holiday.Name == "Martin Luther King Day")
                    {
                        for (int year = 2000; year <= 2020; year++)
                        {
                            Console.WriteLine(holiday.DateInYear(year).Value.ToLongDateString());
                        }
                        //break;
                    }
                }
            }
            ConsoleTool.PressAnyKey();
        }

        private static IEnumerable<HolidayDefinition> LoadHolidaysFromPList(Stream stream)
        {
            if (PropertyListParser.Parse(stream) is NSDictionary data)
            {
                foreach (var item in LoadHolidays(data))
                {
                    var name = item.GetString("name");

                    var i = item.GetInteger("type");
                    if (i.HasValue)
                    {
                        var type = i.Value.ToEnum<HolidayType>();
                        if (type.HasValue)
                        {
                            switch (type.Value)
                            {
                                case HolidayType.GregorianFixed:
                                    {
                                        var day = item.GetInteger("day");
                                        var month = item.GetInteger("month");
                                        if (day.HasValue && month.HasValue)
                                        {
                                            yield return new GregorianFixedHoliday(name, month.Value, day.Value);
                                        }
                                    }
                                    break;
                                case HolidayType.EasterRelative:
                                    {
                                        var offset = item.GetInteger("offset");
                                        if (offset.HasValue) yield return new EasterRelativeHoliday(name, offset.Value);
                                    }
                                    break;
                                case HolidayType.AdventRelative:
                                    {
                                        var offset = item.GetInteger("offset");
                                        if (offset.HasValue) yield return new AdventRelativeHoliday(name, offset.Value);
                                    }
                                    break;
                                case HolidayType.WeekdayInMonth:
                                    {
                                        var month = item.GetInteger("month");
                                        var day = item.GetInteger("day");
                                        var dayOfWeek = day.HasValue ? day.Value - 1.ToEnum<DayOfWeek>() : null;
                                        var ordinal = item.GetInteger("ordinal");
                                        //System.Diagnostics.Debug.Assert(ordinal.HasValue);
                                        //System.Diagnostics.Debug.Assert(ordinal.Value != 0);
                                        var offset = item.GetInteger("offset");
                                        if (month.HasValue && dayOfWeek.HasValue && ordinal.HasValue)
                                        {
                                            yield return new WeekdayInMonthHoliday(name, month.Value, dayOfWeek.Value, ordinal.Value, offset ?? 0);
                                        }
                                    }
                                    System.Diagnostics.Trace.WriteLine($"WeekdayInMonth type for '{name}'.");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else System.Diagnostics.Trace.WriteLine($"Undefined type for '{name}'.");
                    }
                }
            }
        }

        private static T? ToEnum<T>(this int i) where T : struct
        {
            if (Enum.IsDefined(typeof(T), i))
            {
                return (T)Enum.ToObject(typeof(T), i);
            }
            else return null;
        }

        private static int? GetInteger(this NSDictionary dictionary, string name)
        {
            if (dictionary.TryGetValue(name, out var value) && value is NSNumber n)
            {
                if (n.isInteger()) return n.ToInt();
            }
            return null;
        }

        private static string GetString(this NSDictionary dictionary, string name)
        {
            return dictionary.TryGetValue(name, out var value) && value is NSString s ? s.ToString() : null;
        }

        private static IEnumerable<NSDictionary> LoadHolidays(NSDictionary data)
        {
            foreach (var item in data.Values)
            {
                if (item is NSArray array)
                {
                    foreach (var element in array)
                    {
                        if (element is NSDictionary holiday) yield return holiday;
                    }
                }
            }
        }
    }
}
