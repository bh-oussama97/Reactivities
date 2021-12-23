using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
   public  class Edit
    {
        public class Command : IRequest
        {
            public Activity activity { get; set; }

        }


        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.activity).SetValidator(new ActivityValidator());
            }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext datacontext;
            private readonly IMapper mapper;

            public Handler(DataContext datacontext,IMapper mapper)
            {
                this.datacontext = datacontext;
                this.mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await datacontext.Activities.FindAsync(request.activity.Id);

                // activity.Title = request.activity.Title ?? activity.Title;


                mapper.Map(request.activity, activity);

                await datacontext.SaveChangesAsync();
                return Unit.Value;
            }
        }

    }
}
