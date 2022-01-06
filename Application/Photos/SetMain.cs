using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Photos
{
    public class SetMain 
    {

        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext datacontext;
            private readonly IUserAccessor userAccessor;
     
            public Handler(DataContext datacontext, IUserAccessor userAccessor)
            {
                this.datacontext = datacontext;
                this.userAccessor = userAccessor;
          
            }



            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var user = await datacontext.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                if (photo == null) return null;

                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);


                if (currentMain != null)
                    currentMain.IsMain = false;

                photo.IsMain = true;


                var success = await datacontext.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Problem setting main photo");
            }
        }
    }
}
