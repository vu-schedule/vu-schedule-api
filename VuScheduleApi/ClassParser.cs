using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VuScheduleApi
{
    public static class ClassParser
    {
        public static Class[] Parse(string page, string groupId)
        {
            var data = ExtractRelavantPart(page);

            var dataRegex = new Regex(@"{[\S\s]+?}");
            var dataEntries = dataRegex.Matches(data);

            var classes = new List<Class>();

            foreach (Match e in dataEntries)
            {
                if (!e.Value.Contains("data-subject"))
                    continue;

                classes.Add(new Class
                {
                    End = DateTime.Parse(ExtractData(e.Value, "\"end\":")),
                    Start = DateTime.Parse(ExtractData(e.Value, "\"start\":")),
                    Location = ExtractData(e.Value, "data-rooms"),
                    Teacher = ExtractData(e.Value, "data-academics"),
                    Type = ExtractData(e.Value, "data-eventtype").ToLower().FirstCharToUpper(),
                    Title = ExtractData(e.Value, "data-subject"),
                    IsMandatory = ExtractData(e.Value, "data-subjecttype") == "Privalomasis",
                    Subgroup = GetSubgroup(ExtractData(e.Value, "data-subgroups").Trim(), groupId)
                });
            }

            NormalizeSubgroups(classes);

            return classes.ToArray();
        }

        private static void NormalizeSubgroups(List<Class> classes)
        {
            var groupedClasses = classes.GroupBy(x => x.Title);

            foreach (var g in groupedClasses)
            {
                var subgroups = g.GroupBy(x => x.Subgroup).Select(x => x.First()).Sum(x => x.Subgroup);

                if (subgroups < 3)
                {
                    foreach (var s in g)
                    {
                        s.Subgroup = 0;
                    }
                }
            }
        }

        private static string ExtractData(string source, string key)
        {
            source = source.GetStringAfter(key);

            if (source.StartsWith("='<a"))
            {
                return source.GetStringAfter(">")
                           .GetFirstStringBefore("</a>");
            }
            else if (source.StartsWith("='"))
            {
                return source.GetStringAfter("'")
                             .GetFirstStringBefore("'");
            }

            return source.GetStringAfter(" \"")
                         .GetFirstStringBefore("\"");
        }

        private static string ExtractRelavantPart(string data)
        {
            var regex = new Regex(@"events: \[[\S\s]*],");
            var eventsRaw = regex.Match(data).Value;
            return eventsRaw.Substring(8, eventsRaw.Length - 9);
        }
        private static int? GetSubgroup(string subgroup, string group)
        {
            if (subgroup == "")
                return null;

            subgroup = subgroup.Replace("Pogrupiai: ", "");

            if (subgroup.Length == 2)
            {
                return int.Parse(subgroup.Last().ToString());
            }

            if (subgroup.Length == 5)
            {
                var regex = new Regex(@"-k-\d-gr");
                var value = regex.Match(group).Value.Replace("-k-", "").Replace("-gr", "");

                var elements = subgroup.Split(",");

                if (elements.First().StartsWith(value))
                {
                    return int.Parse(elements.First().Last().ToString());
                }

                return int.Parse(elements.Last().Last().ToString());

            }

            return int.Parse(subgroup);
        }

    }
}