using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace VuScheduleApi.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private SubjectsService _service;

        public SubjectsController(SubjectsService service)
        {
            _service = service;
        }

        [HttpGet("{groupId}")]
        public async Task<Subject[]> Get(string groupId)
        {
            try
            {
                return await _service.GetSubjectsAsync("mif", groupId);
            }
            catch
            {
                return null;
            }
        }
    }
}
