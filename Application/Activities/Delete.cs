using Domain;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
   public class Delete
    {

        public class Command : IRequest

        {
            public Guid Id { get; set; }
        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext datacontext;

            public Handler(DataContext _datacontext)
            {
                datacontext = _datacontext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Activity activity = datacontext.Activities.FindAsync(request.Id).Result;

                datacontext.Activities.Remove(activity);

                await datacontext.SaveChangesAsync();


                return Unit.Value;
                    
                    }
        }

    }
}
