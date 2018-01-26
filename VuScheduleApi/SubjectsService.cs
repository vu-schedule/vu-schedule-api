using System;
using System.Linq;
using System.Threading.Tasks;
using VuScheduleApi.Controllers;

namespace VuScheduleApi
{
    public class SubjectsService
    {
        private WebClient _client;

        public SubjectsService(WebClient client)
        {
            _client = client;
        }

        public async Task<Subject[]> GetSubjectsAsync(string facultyId, string groupId)
        {
            var page = await _client.GetClassesPageAsync(facultyId, groupId);

            var classes = ClassParser.Parse(page, groupId);

            return classes.GroupBy(x => x.Title)
                          .Select(x => {

                              var subgroups = x.GroupBy(y => y.Subgroup).Select(y=>y.First().Subgroup).ToArray();
                              int? subgroupsCount = null;

                              if(subgroups.Count() != 1)
                              {
                                  subgroupsCount = subgroups.Count();

                                  if (subgroups.Contains(null))
                                      subgroupsCount -= 1;
                              }

                              return new Subject
                              {
                                  Title = x.First().Title,
                                  SubgroupsCount = subgroupsCount,
                                  IsMandatory = x.First().IsMandatory
                              };
                          })
                          .OrderByDescending(x=>x.IsMandatory)
                          .ThenBy(x=>x.Title)
                          .ToArray();
        }
    }
}