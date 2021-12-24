﻿using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Persistence;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace API.Controllers
{
        public class ActivitiesController : BaseApiController
    {
       
  
        [HttpGet]

        public async Task<IActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddActivity(Activity act)
        {
            return HandleResult(await Mediator.Send(new Create.Command { activity = act }));
        }




        [HttpPut("update/{id}")]

        public async Task<IActionResult> updateactivity(Guid id, Activity activity)
        {
            
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { activity = activity }));
        }


        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> deleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

      


        [HttpGet("{id}")]
        //IActionResult allows us to return response of http request
        public async Task<IActionResult> GetAct(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));


        }


    }
}
