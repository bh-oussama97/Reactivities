using Application.Photos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PhotoController : BaseApiController
    {
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddPhoto.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new DeletePhoto.Command
            {
                Id = id
            }));
        }


        [HttpPost("setMain/{id}")]

        public async Task<IActionResult> setMain(string id)

        {
            return HandleResult(await Mediator.Send(new SetMain.Command { Id = id }));
        }


    }
}