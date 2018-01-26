using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace VuScheduleApi
{
    public class WebClient
    {
        private string _baseUrl;
        private string _sessionId;

        public WebClient(string baseUrl, string sessionId)
        {
            _baseUrl = baseUrl;
            _sessionId = sessionId;
        }

        public async Task<string> GetProgramsPageAsync(string facultyId)
        {
            return await
                    (await _baseUrl
                           .AppendPathSegment(facultyId)
                           .WithCookie("sessionid", _sessionId)
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
                           .WithCookie("sessionid", _sessionId)
                           .GetAsync())
                   .Content
                   .ReadAsStringAsync();
        }
    }
}