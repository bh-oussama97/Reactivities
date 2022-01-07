using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class List
    {

        public class Query : IRequest<Result<List<CommentDTO>>>
        {

            public Guid ActivityId { get; set; }

        }


        public class Handler : IRequestHandler<Query, Result<List<CommentDTO>>>

        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<CommentDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {

              //get comments by activity id comming from request ordered by creation date
              //ProjectTo CommentDTO using mapper configuration to transform list of comments class to list of comments DTO
                var comments = await context.Comments
                    .Where(x => x.Activity.Id == request.ActivityId)
                    .OrderBy(x => x.CreatedAt)
                    .ProjectTo<CommentDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();


                return Result<List<CommentDTO>>.Success(comments);

            }
        }

    }
}
