using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Comments
{
   public class Create
    {

        public class Command : IRequest<Result<CommentDTO>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }

        }


        public class Handler : IRequestHandler<Command, Result<CommentDTO>>
        {
            private readonly DataContext datacontext;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext datacontext,IMapper mapper,IUserAccessor userAccessor)
            {
                this.datacontext = datacontext;
                this.mapper = mapper;
                this.userAccessor = userAccessor;
            }

            public async Task<Result<CommentDTO>> Handle(Command request, CancellationToken cancellationToken)
            {

                //get activity by id 
                var activity = await datacontext.Activities.FindAsync(request.ActivityId);

                if (activity == null) return null;


                // populate image property inside user that we are going to return by user (userAccessor.GetUsername())
                var user = await datacontext.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());


                //create the comment object to add 
                //request.body comming from request body in format json
                var comment = new Comment
                {
                    Author = user,
                    Activity = activity,
                    Body = request.Body
                };


             
                activity.Comments.Add(comment);

                //use await to ensure that any asynchronous operation have completed before this instruction.
                // var success will be a boolean result of saving changes in database
                var success = await datacontext.SaveChangesAsync() > 0;

                //use mapper.map to map object comment to commentDTO
                if (success) return Result<CommentDTO>.Success(mapper.Map<CommentDTO>(comment));
                

                //else return 
                return Result<CommentDTO>.Failure("Failed to add comment");


            }
        }
    }
}
