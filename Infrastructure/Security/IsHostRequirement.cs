using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
   public class IsHostRequirement : IAuthorizationRequirement
    {

    }


    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext datacontext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IsHostRequirementHandler(DataContext datacontext, IHttpContextAccessor httpContextAccessor)
        {
            this.datacontext = datacontext;
            this.httpContextAccessor = httpContextAccessor;
        }

     

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //user is not authorized to do operation
            if (userID == null) return Task.CompletedTask;


            //get activity id from parameter (routes values)
            var activityId = Guid.Parse(httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString());

            // find activityAttendee from the many to many table ActivityAttendees 
            var attendee = datacontext.ActivityAttendees.

                AsNoTracking()
                .SingleOrDefaultAsync(w => w.UserId == userID && w.ActivityId == activityId).Result;

            if (attendee == null) return Task.CompletedTask;


            // check if user in table activitiesAttendees is host or not
            if (attendee.isHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
