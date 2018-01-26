using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace VuScheduleApi
{
    public class WebClient
    {
        private string _baseUrl;
        private string _semesterId;

        public WebClient(string baseUrl, string semesterId)
        {
            _baseUrl = baseUrl;
            _semesterId = semesterId;
        }

        public async Task<string> GetProgramsPageAsync(string facultyId)
        {
            return await
                    (await _baseUrl
                           .AppendPathSegment(facultyId)
                           .WithCookie("sessionid", _semesterId)
                           .GetAsync())
                   .Content
                   .ReadAsStringAsync();
        }

        public async Task<string> GetClassesPageAsync(string facultyId, string groupId)
        {
            return await
                    (await _baseUrl
                           .AppendPathSegment(facultyId)
                           .AppendPathSegment("groups")
                           .AppendPathSegment(groupId)
                           .WithCookie("used_semester", _semesterId)
                           .GetAsync())
                   .Content
                   .ReadAsStringAsync();
        }
    }
}