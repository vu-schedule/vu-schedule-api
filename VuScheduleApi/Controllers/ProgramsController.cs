﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace VuScheduleApi.Controllers
{
    [Route("api/[controller]")]
    public class ProgramsController : Controller
    {
        private StudyService _service;

        public ProgramsController(StudyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<StudyProgram[]> Get()
        {
            try
            {
                return await _service.GetProgramsAsync("mif");
            }
            catch
            {
                return null;
            }
        }
    }
}
