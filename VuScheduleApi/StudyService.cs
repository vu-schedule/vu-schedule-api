using System;
using System.Threading.Tasks;
using VuScheduleApi.Controllers;

namespace VuScheduleApi
{
    public class StudyService
    {
        private WebClient _client;

        public StudyService(WebClient client)
        {
            _client = client;
        }

        public async Task<StudyProgram[]> GetProgramsAsync(string facultyId)
        {
            var page = await _client.GetProgramsPageAsync(facultyId);

            return ProgramParser.Parse(page);
        }
    }
}