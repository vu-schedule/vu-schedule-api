using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VuScheduleApi.Controllers;

namespace VuScheduleApi
{
    public static class ProgramParser
    {
        public static StudyProgram[] Parse(string page)
        {
            var data = ExtractRelavantPart(page);

            var document = new HtmlDocument();
            document.LoadHtml(data);

            var nodes = document.DocumentNode.SelectNodes("/div/div").Select(x => { var d = new HtmlDocument(); d.LoadHtml(x.InnerHtml); return d; }).ToArray();

            var courses = new List<StudyProgram>();

            foreach (var n in nodes)
            {
                var studyProgram = new StudyProgram();
                studyProgram.Title = n.DocumentNode.SelectSingleNode("//h4").FirstChild.InnerHtml;

                if (studyProgram.Title == "Bendrosios universitetinės studijos" || studyProgram.Title == "Erasmus")
                    continue;

                var courseNodes = n.DocumentNode.SelectNodes("/div/div").Select(x => { var d = new HtmlDocument(); d.LoadHtml(x.InnerHtml); return d; }).ToArray();

                foreach (var cn in courseNodes)
                {
                    var linksNodes = cn.DocumentNode.SelectNodes("//a");
                    var year = new StudyYear
                    {
                        Title = CleanString(linksNodes.First().InnerHtml)
                    };
                    var groups = linksNodes.Skip(1).Select(x => new StudyGroup
                    {
                        Title = CleanString(x.InnerHtml),
                        Id = x.Attributes.First().Value.GetStringBefore("/").GetLastStringAfter("/")
                    })
                    .ToArray();
                    year.StudyGroups.AddRange(groups);
                    studyProgram.StudyYears.Add(year);
                }

                courses.Add(studyProgram);
            }
            return courses.ToArray();
        }

        private static string ExtractRelavantPart(string data)
        {
            var regex = new Regex(@"bakalauro, Nuolatinė[\S\s]+?magistrantūros, Nuolatinė");
            var match = regex.Match(data);
            return "<div>" + match.Value
                                       .GetStringFrom("<div")
                                       .GetStringBefore("</div>");
        }

        private static string CleanString(string source)
        {
            return source.Replace("\n", "").Trim();
        }
    }
}