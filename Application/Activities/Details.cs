using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
   public class Details
    {

      
        public class Query : IRequest<Result<ActivitiesDTO>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivitiesDTO>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper mapper;

            public Handler(DataContext dataContext,IMapper mapper)
            {
                this._dataContext = dataContext;
                this.mapper = mapper;
            }

            public async Task<Result<ActivitiesDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities
           .ProjectTo<ActivitiesDTO>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x=> x.Id == request.Id);

                return Result<ActivitiesDTO>.Success(activity);
            }
        }



    }
}
