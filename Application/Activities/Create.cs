﻿using Application.Core;
using Domain;
using FluentValidation;
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


        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext datacontext;

            public Handler(DataContext datacontext)
            {
                this.datacontext = datacontext;
            }   

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                datacontext.Activities.Add(request.activity);
                var result = await datacontext.SaveChangesAsync() > 0;

                if (!result)

                    return Result<Unit>.Failure("Failed to create activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
