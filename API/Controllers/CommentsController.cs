using Application.Comments;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CommentsController : BaseApiController
    {


        [HttpPost("add/{activityid}")]

        public async Task<IActionResult> CreateComment(Guid activityid,CommentDTO commentDTO)
        {
            return HandleResult(await Mediator.Send(new Create.Command
            {
                Body = commentDTO.Body,
                ActivityId = activityid
            }));
        }


        [HttpGet("{activityid}")]

        public async Task<IActionResult> GetCommentsList(Guid activityid)
        {

            return HandleResult(await Mediator.Send(new List.Query { ActivityId = activityid }));
        }

    }
}
