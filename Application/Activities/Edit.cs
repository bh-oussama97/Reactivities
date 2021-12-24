using Application.Core;
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
        public class Command : IRequest<Result<Unit>>
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
        public class Handler : IRequestHandler<Command,Result<Unit>>
        {
            private readonly DataContext datacontext;
            private readonly IMapper mapper;

            public Handler(DataContext datacontext,IMapper mapper)
            {
                this.datacontext = datacontext;
                this.mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await datacontext.Activities.FindAsync(request.activity.Id);

                if (activity == null) return null;

                // activity.Title = request.activity.Title ?? activity.Title;


                mapper.Map(request.activity, activity);

               var result =  await datacontext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("failed to update activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }

    }
}
