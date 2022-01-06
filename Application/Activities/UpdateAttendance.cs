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

namespace Application.Activities
{
   public class UpdateAttendance
    {

        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext datacontext;

            public Handler(DataContext datacontext,IUserAccessor userAcessor)
            {
                this.datacontext = datacontext;
                UserAcessor = userAcessor;
            }

            public IUserAccessor UserAcessor { get; }

            public  async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await datacontext.Activities
                    .Include(a => a.Attendees).ThenInclude(u => u.User)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);


                if (activity == null) return null;

                var user = await datacontext.Users.FirstOrDefaultAsync(x => x.UserName == UserAcessor.GetUsername());

                if (user == null) return null;

                var hostname = activity.Attendees.FirstOrDefault(x => x.isHost)?.User?.UserName;

                var attendance = activity.Attendees.FirstOrDefault(x => x.User.UserName == user.UserName);

                if (attendance != null && hostname == user.UserName)

                    activity.IsCancelled = !activity.IsCancelled;

                if (attendance != null && hostname != user.UserName)

                    activity.Attendees.Remove(attendance);

                if (attendance == null)

                {
                    attendance = new Domain.ActivityAttendee
                    {
                        User = user,
                        Activity = activity,
                        isHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await datacontext.SaveChangesAsync() > 0;
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");

            }


        }

    }

}
