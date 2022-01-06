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
   public class List
    {

        public class Query : IRequest<Result<List<ActivitiesDTO>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActivitiesDTO>>>
        {
            private readonly DataContext _datacontext;
            private readonly IMapper mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                this._datacontext = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<ActivitiesDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _datacontext.Activities
                .ProjectTo<ActivitiesDTO>(mapper.ConfigurationProvider)
                    //include users in activities list
                    //.Include(a => a.Attendees)
                    //user informations
                    //.ThenInclude(u => u.User) 
                    .ToListAsync();


                //var activitiesToReturn = mapper.Map<List<ActivitiesDTO>>(activities);


                return Result < List <ActivitiesDTO>>.Success(activities) ;
            }
        }


    }
}
