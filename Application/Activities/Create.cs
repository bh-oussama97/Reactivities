using Domain;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Create
    {

       public class Command : IRequest
        {
            public Activity activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext datacontext;

            public Handler(DataContext datacontext)
            {
                this.datacontext = datacontext;
            }   

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                datacontext.Activities.Add(request.activity);
                await datacontext.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
